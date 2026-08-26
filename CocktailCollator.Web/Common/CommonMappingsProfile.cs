using AutoMapper;
using CocktailCollator.Application.Models;
using CocktailCollator.Web.Common.Generics;

namespace CocktailCollator.Web.Common;

public class CommonMappingsProfile : Profile
{
    public CommonMappingsProfile()
    {
        _ = this.CreateMap<DocumentInputProperty, IFormFile>()
            .ConstructUsing(input => input.Output!);

        _ = this.CreateMap<DocumentInputProperty, DocumentModel>()
            .ForMember(d => d.ExistingDocumentId, o => o.MapFrom(s => s.Existing!.Id))
            .ForMember(d => d.NewDocument, o => o.MapFrom(s => s.Output));

        _ = this.CreateMap<DocumentInputPropertyList, IEnumerable<IFormFile>>()
            .ConstructUsing(inputs => inputs.AsInputs().Where(input => input.Output != null).Select(input => input.Output!).ToList());

        _ = this.CreateMap<DocumentInputPropertyList, IEnumerable<DocumentModel>>()
            .ConstructUsing((inputs, context) => inputs.AsInputs().Select(input => context.Mapper.Map<DocumentModel>(input)).ToList());

        this.RegisterInputPropertyMappings();
    }

    // I'm not sure this is the best solution, but it works for now.
    /// <summary>
    /// Finds all InputProperty<T> types in the assembly and creates a mapping for each of them to T.
    /// </summary>
    private void RegisterInputPropertyMappings()
    {
        var _InputPropertyTypes = typeof(CommonMappingsProfile).Assembly.GetTypes()
            .SelectMany(t => t.GetProperties())
            .Select(p => p.PropertyType)
            .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(InputProperty<>))
            .Distinct();

        var _CreateMapMethod = typeof(Profile).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
            .First(m => m.Name == nameof(CreateMap) && m.IsGenericMethod && m.GetParameters().Length == 0);

        var _ConfigureMethod = typeof(CommonMappingsProfile).GetMethod(nameof(ConfigureConvertUsing), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        foreach (var _Type in _InputPropertyTypes)
        {
            var _InnerType = _Type.GetGenericArguments()[0];

            var _GenericCreateMap = _CreateMapMethod.MakeGenericMethod(_Type, _InnerType);
            var _Map = _GenericCreateMap.Invoke(this, null);

            var _GenericConfigure = _ConfigureMethod!.MakeGenericMethod(_InnerType);
            _GenericConfigure.Invoke(null, new[] { _Map });
        }
    }

    private static void ConfigureConvertUsing<T>(IMappingExpression<InputProperty<T>, T> map)
    {
        map.ConvertUsing(x => x == null ? default! : x.Input);
    }
}
