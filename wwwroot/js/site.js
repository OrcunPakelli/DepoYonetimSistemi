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

function printTable() {
    const table = document.getElementById("logTable");

    if (!table) {
        alert("Yazdırılacak tablo bulunamadı.");
        return;
    }

    const newWindow = window.open("", "", "width=900,height=650");

    newWindow.document.write(`
        <!DOCTYPE html>
        <html lang="tr">
        <head>
            <meta charset="UTF-8">
            <title>İşlem Geçmişi</title>

            <!-- Bootstrap -->
            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css">

            <style>
                body {
                    padding: 20px;
                    font-family: Arial, sans-serif;
                }

                h2 {
                    text-align: center;
                    margin-bottom: 20px;
                }

                table {
                    width: 100%;
                    border-collapse: collapse;
                }

                th, td {
                    border: 1px solid #ddd;
                    padding: 8px;
                    font-size: 13px;
                }

                th {
                    background-color: #f2f2f2;
                }
            </style>
        </head>
        <body>
            <h2>İşlem Geçmişi</h2>
            ${table.outerHTML}
        </body>
        </html>
    `);

    newWindow.document.close();
    newWindow.focus();
    newWindow.print();
    newWindow.close();
}

