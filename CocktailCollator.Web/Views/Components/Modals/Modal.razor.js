export function openModal(modalId) {
    const modalElement = document.getElementById(modalId);
    const modal = window.bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.show();
}

export function closeModal(modalId) {
    const modalElement = document.getElementById(modalId);
    const modal = window.bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.hide();
}
