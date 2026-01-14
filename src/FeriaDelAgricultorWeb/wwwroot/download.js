// wwwroot/js/download.js
// Descarga un archivo desde texto (CSV/TXT) usando Blob.
// Funciona en Blazor Server (InteractiveServer) vía JS Interop.

window.downloadFileFromText = (fileName, contentType, content) => {
    try {
        const blob = new Blob([content], { type: contentType || "application/octet-stream" });
        const url = URL.createObjectURL(blob);

        const a = document.createElement("a");
        a.href = url;
        a.download = fileName || "reporte.txt";
        document.body.appendChild(a);
        a.click();

        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    } catch (e) {
        console.error("downloadFileFromText error:", e);
        alert("No se pudo descargar el archivo. Revisa la consola del navegador.");
    }
};
