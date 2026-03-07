export function openModal(modalId) {
    const modalElement = document.getElementById(modalId);
    const modal = new window.bootstrap.Modal(modalElement);
    modal.show();
}

export function closeModal(modalId) {
    const modalElement = document.getElementById(modalId);
    const modal = new window.bootstrap.Modal(modalElement);
    modal.hide();
}
