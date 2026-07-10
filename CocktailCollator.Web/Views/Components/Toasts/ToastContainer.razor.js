export function showToast(toastId, dotNetReference) {
    const toastElement = document.getElementById(toastId);
    if (toastElement) {
        const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastElement);
        
        toastElement.addEventListener('hidden.bs.toast', () => {
            dotNetReference.invokeMethodAsync('RemoveToast', toastId);
        }, { once: true });
        
        toastBootstrap.show();
    }
}