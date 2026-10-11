export function getTimezoneOffsetMinutes() {
    // JS returns minutes UTC is ahead of local time, but Blazor expects minutes local time is ahead of UTC, so reverse it.
    return -(new Date().getTimezoneOffset());
}

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