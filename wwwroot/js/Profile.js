//Kullanıcı sil
document.addEventListener("DOMContentLoaded", function () {

    const deleteBtn = document.getElementById("btnDeleteSelected");
    const selectedIdsInput = document.getElementById("selectedIds");
    const selectedCount = document.getElementById("selectedCount");
    const deleteForm = document.getElementById("deleteSelectedForm");
    const checkAll = document.getElementById("checkAll");

    function getSelectedIds() {
        return Array.from(document.querySelectorAll(".user-check:checked"))
            .map(cb => cb.value);
    }

    function refreshDeleteButton() {
        const ids = getSelectedIds();
        if (deleteBtn) {
            deleteBtn.disabled = ids.length === 0;
        }
    }

    document.addEventListener("change", function (e) {
        if (e.target.classList.contains("user-check")) {
            refreshDeleteButton();
        }
    });

    if (deleteBtn) {
        deleteBtn.addEventListener("click", function () {
            const ids = getSelectedIds();
            if (selectedCount) selectedCount.textContent = ids.length;
            if (selectedIdsInput) selectedIdsInput.value = ids.join(",");
        });
    }

    const confirmBtn = document.getElementById("confirmDeleteSelected");
    if (confirmBtn && deleteForm) {
        confirmBtn.addEventListener("click", function () {
            deleteForm.submit();
        });
    }

    refreshDeleteButton();
});

document.addEventListener("click", function (e) {
    const btn = e.target.closest(".js-open-passmodal");
    if (!btn) return;

    document.getElementById("modalUserId").value = btn.getAttribute("data-userid");
    document.getElementById("modalUsername").textContent = btn.getAttribute("data-username");
});

const checkAll = document.getElementById("checkAll");
const deleteBtn = document.getElementById("btnDeleteSelected");

function toggleDeleteBtn() {
    const anyChecked = document.querySelectorAll(".user-check:checked").length > 0;
    deleteBtn.disabled = !anyChecked;
}

checkAll?.addEventListener("change", function () {
    document.querySelectorAll(".user-check").forEach(cb => {
        cb.checked = checkAll.checked;
    });
    toggleDeleteBtn();
});

document.addEventListener("change", function (e) {
    if (e.target.classList.contains("user-check")) {
        toggleDeleteBtn();
    }
});