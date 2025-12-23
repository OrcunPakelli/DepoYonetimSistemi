function openNav() {
  document.getElementById("Sidepanel").style.width = "300px";
}

function closeNav() {
  document.getElementById("Sidepanel").style.width = "0";
}

    function printTable() {
        const tableHtml = document.getElementById("logTable").outerHTML;

        const printWindow = window.open("", "", "width=900,height=700");

        printWindow.document.write(`
            <html>
                <head>
                    <title>İşlem Logları</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                        }
                        table {
                            width: 100%;
                            border-collapse: collapse;
                        }
                        th, td {
                            border: 1px solid #000;
                            padding: 8px;
                            text-align: left;
                        }
                        th {
                            background-color: #f2f2f2;
                        }
                    </style>
                </head>
                <body>
                    <h3>İşlem Logları</h3>
                    ${tableHtml}
                </body>
            </html>
        `);

        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }