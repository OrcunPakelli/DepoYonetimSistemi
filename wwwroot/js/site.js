document.addEventListener("DOMContentLoaded", () => {
    const btn = document.getElementById("filterToggleBtn");
    const panel = document.getElementById("filterPanel");

    console.log(btn, panel); // TEST

    if (!btn || !panel) return;

    btn.onclick = () => {
        panel.classList.toggle("open");
        btn.classList.toggle("active");
    };
});
