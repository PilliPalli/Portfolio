// Theme management
function applyTheme(theme) {
    const root = document.documentElement;
    
    switch (theme) {
        case 'Dark':
            root.style.setProperty('--terminal-bg', '#121212');
            root.style.setProperty('--terminal-text', '#33ff33');
            root.style.setProperty('--terminal-border', '#33ff33');
            root.style.setProperty('--terminal-header-bg', '#121212');
            root.style.setProperty('--terminal-header-text', '#33ff33');
            root.style.setProperty('--body-bg', '#121212');
            break;
        case 'Light':
            root.style.setProperty('--terminal-bg', '#f0f0f0');
            root.style.setProperty('--terminal-text', '#121212');
            root.style.setProperty('--terminal-border', '#121212');
            root.style.setProperty('--terminal-header-bg', '#e0e0e0');
            root.style.setProperty('--terminal-header-text', '#121212');
            root.style.setProperty('--body-bg', '#f0f0f0');
            break;
        case 'Hacker':
            root.style.setProperty('--terminal-bg', '#000000');
            root.style.setProperty('--terminal-text', '#00ff00');
            root.style.setProperty('--terminal-border', '#00ff00');
            root.style.setProperty('--terminal-header-bg', '#000000');
            root.style.setProperty('--terminal-header-text', '#00ff00');
            root.style.setProperty('--body-bg', '#000000');
            break;
        case 'Retro':
            root.style.setProperty('--terminal-bg', '#2b2b2b');
            root.style.setProperty('--terminal-text', '#ff8c00');
            root.style.setProperty('--terminal-border', '#ff8c00');
            root.style.setProperty('--terminal-header-bg', '#2b2b2b');
            root.style.setProperty('--terminal-header-text', '#ff8c00');
            root.style.setProperty('--body-bg', '#2b2b2b');
            break;
        case 'Custom':
            const customBg = localStorage.getItem('terminal_custom_bg') || '#121212';
            const customText = localStorage.getItem('terminal_custom_text') || '#33ff33';
            const customBorder = localStorage.getItem('terminal_custom_border') || '#33ff33';
            
            root.style.setProperty('--terminal-bg', customBg);
            root.style.setProperty('--terminal-text', customText);
            root.style.setProperty('--terminal-border', customBorder);
            root.style.setProperty('--terminal-header-bg', customBg);
            root.style.setProperty('--terminal-header-text', customText);
            root.style.setProperty('--body-bg', customBg);
            break;
    }
}

// Matrix animation
function startMatrixAnimation() {
    const canvas = document.createElement('canvas');
    const terminal = document.querySelector('.terminal-body');
    terminal.innerHTML = '';
    terminal.appendChild(canvas);
    
    canvas.width = terminal.offsetWidth;
    canvas.height = terminal.offsetHeight;
    
    const ctx = canvas.getContext('2d');
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$+-*/=%"\'#&_(),.;:?!\\|{}<>[]^~';
    const columns = Math.floor(canvas.width / 20);
    const drops = [];
    
    for (let i = 0; i < columns; i++) {
        drops[i] = 1;
    }
    
    ctx.fillStyle = 'rgba(0, 0, 0, 0.05)';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    
    ctx.fillStyle = '#0f0';
    ctx.font = '15px monospace';
    
    function draw() {
        ctx.fillStyle = 'rgba(0, 0, 0, 0.05)';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        
        ctx.fillStyle = '#0f0';
        ctx.font = '15px monospace';
        
        for (let i = 0; i < drops.length; i++) {
            const text = characters.charAt(Math.floor(Math.random() * characters.length));
            ctx.fillText(text, i * 20, drops[i] * 20);
            
            if (drops[i] * 20 > canvas.height && Math.random() > 0.975) {
                drops[i] = 0;
            }
            
            drops[i]++;
        }
    }
    
    return setInterval(draw, 33);
}

// Resume PDF generation and download
function downloadResume() {
    // In a real application, you would generate a PDF using a library like jsPDF
    // For this example, we'll create a simple PDF with basic information
    
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();
    
    doc.setFontSize(22);
    doc.text('RESUME', 105, 20, { align: 'center' });
    
    doc.setFontSize(16);
    doc.text('PERSONAL INFORMATION', 20, 40);
    doc.setFontSize(12);
    doc.text('Name: Dein Name', 20, 50);
    doc.text('Email: deine-email@example.com', 20, 60);
    doc.text('LinkedIn: linkedin.com/in/deinname', 20, 70);
    doc.text('GitHub: github.com/deinname', 20, 80);
    
    doc.setFontSize(16);
    doc.text('SKILLS', 20, 100);
    doc.setFontSize(12);
    doc.text('- C# (5/5)', 20, 110);
    doc.text('- Blazor (4/5)', 20, 120);
    doc.text('- SQL & Dapper (4/5)', 20, 130);
    doc.text('- ASP.NET Core (4/5)', 20, 140);
    doc.text('- Git & CI/CD (4/5)', 20, 150);
    
    doc.setFontSize(16);
    doc.text('EXPERIENCE', 20, 170);
    doc.setFontSize(12);
    doc.text('Software Developer | Company Name | 2020 - Present', 20, 180);
    doc.text('- Developed and maintained web applications using C# and Blazor', 20, 190);
    doc.text('- Implemented database solutions using SQL and Dapper', 20, 200);
    
    doc.save('resume.pdf');
}

// Load jsPDF library dynamically
function loadJsPDF() {
    return new Promise((resolve, reject) => {
        if (window.jspdf) {
            resolve();
            return;
        }
        
        const script = document.createElement('script');
        script.src = 'https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js';
        script.onload = () => resolve();
        script.onerror = () => reject(new Error('Failed to load jsPDF'));
        document.head.appendChild(script);
    });
}

// Initialize
window.initTerminal = async function() {
    try {
        await loadJsPDF();
        console.log('Terminal initialized');
    } catch (error) {
        console.error('Error initializing terminal:', error);
    }
};

// Text animation
window.animateText = function(text, elementId, speed = 50) {
    return new Promise(resolve => {
        const element = document.getElementById(elementId);
        if (!element) {
            resolve();
            return;
        }
        
        element.textContent = '';
        let i = 0;
        
        const interval = setInterval(() => {
            if (i < text.length) {
                element.textContent += text.charAt(i);
                i++;
            } else {
                clearInterval(interval);
                resolve();
            }
        }, speed);
    });
};

// Matrix animation control
let matrixInterval = null;

window.startMatrix = function() {
    if (matrixInterval) {
        clearInterval(matrixInterval);
    }
    matrixInterval = startMatrixAnimation();
    return true;
};

window.stopMatrix = function() {
    if (matrixInterval) {
        clearInterval(matrixInterval);
        matrixInterval = null;
    }
    return true;
};
