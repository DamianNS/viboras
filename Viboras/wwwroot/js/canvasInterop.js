export function initialize(canvas, pixelSize) {
    if (!canvas) return;
    canvas._ctx = canvas.getContext('2d');
    canvas._pixelSize = pixelSize || 6;
}

export function clear(canvas) {
    if (!canvas || !canvas._ctx) return;
    canvas._ctx.clearRect(0, 0, canvas.width, canvas.height);
}

export function drawCells(canvas, cells) {
    if (!canvas || !canvas._ctx) return;
    const ctx = canvas._ctx;
    const ps = canvas._pixelSize || 6;
    for (const c of cells) {
        ctx.fillStyle = c.color || '#000';
        ctx.fillRect(c.x * ps, c.y * ps, ps, ps);
    }
}

export function drawGrid(canvas, gridW, gridH) {
    if (!canvas || !canvas._ctx) return;
    const ctx = canvas._ctx;
    const ps = canvas._pixelSize || 6;
    ctx.strokeStyle = '#e0e0e0';
    for (let x = 0; x <= gridW; x++) {
        const xx = x * ps;
        ctx.beginPath();
        ctx.moveTo(xx, 0);
        ctx.lineTo(xx, gridH * ps);
        ctx.stroke();
    }
    for (let y = 0; y <= gridH; y++) {
        const yy = y * ps;
        ctx.beginPath();
        ctx.moveTo(0, yy);
        ctx.lineTo(gridW * ps, yy);
        ctx.stroke();
    }
}
