export function showAccordionSection(sectionId, animate = true) {
    const section = document.getElementById(sectionId);
    if (section) {
        if (!animate) {
            section.classList.add('show');
            return;
        }

        section.classList.remove('show'); // So that it can be added via the animation call.
        const collapse = window.bootstrap.Collapse.getOrCreateInstance(section, { toggle: false });
        collapse.show();
    }
}

export function hideAccordionSection(sectionId) {
    const section = document.getElementById(sectionId);
    if (section) {
        section.classList.add('show'); // So that it can be removed via the animation call.
        const collapse = window.bootstrap.Collapse.getOrCreateInstance(section, { toggle: false });
        collapse.hide();
    }
}
