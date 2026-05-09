using Microsoft.AspNetCore.Identity;

namespace CocktailCollator.Infrastructure.Persistence.Models;

public class CocktailUser : IdentityUser<Guid>
{
}

// the current issue that is appearing seems to be from the fact that users do not exist in the database
// however asp wants to make alot of tables for the identity system, so need to check how I can get it to work properly but cleanly
// run a migration (not the update tho) and see if it adds all the tables or if it just adds User

// i did the migration, seemed to work nicely
// however apparently Program should also include the role model (which also note the fact that there is a role model)
// also it would seem that user does not need to be a domain entity, which means it could be taken out of domain and possibly even be treated like a view model
// see the gpt chat
// currently breakpointing in Home.razor to see results, can register but not login so far

// seems the problem with login is that i am using blazor server, and therefore it cannot break out of the cycle
// I need to be able to change the rendermode (currently found in app.razor, however applicable per page)
// would be good if I could leave the default as server, but set static specifically for auth pages
// also still need to check that the login did actually work - it just didn't exception and returned Succeeded, and I saw that as a pass lol

// okay so having App decide rendermode based on the route seems to work, login and register pages are static, and the rest are server
// the `[SupplyParameterFromForm]` is actually working based on when the editcontext form is posted, and thats how the method runs, basically like an endpoint
// I have discovered that I do not have to expose external endpoints (at least initially)
// now I should look into better placing User and also creating a role model
// also try to move the stuff in program into the dependency injector
// also possibly once im done, make a new branch (rename this one to old?) and properly do the migrations?