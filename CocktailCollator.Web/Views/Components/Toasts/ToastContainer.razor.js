export function getLocalTime(utcTime) {
    // Convert string date to JavaScript Date
    const date = new Date(utcTime);
    if (isNaN(date.getTime())) {
        return utcTime;
    }

    // Add the local time offset
    const localDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
    return localDate.toISOString().slice(0, -1);
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