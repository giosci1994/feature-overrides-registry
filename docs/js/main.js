// Feature Overrides — landing page
// One language at a time (English / Italian): every translated element carries
// data-en and data-it. The choice is remembered in this browser.
document.addEventListener('DOMContentLoaded', () => {
  const root = document.documentElement;
  const langBtn = document.getElementById('lang-toggle');
  const texts = document.querySelectorAll('[data-en]');
  const metaDesc = document.querySelector('meta[name="description"]');
  const KEY = 'feature-overrides-lang';

  function saved() {
    try { return localStorage.getItem(KEY); } catch (e) { return null; }
  }

  function remember(lang) {
    try { localStorage.setItem(KEY, lang); } catch (e) { /* private mode: no memory */ }
  }

  function apply(lang) {
    texts.forEach((el) => {
      const value = el.dataset[lang];
      if (value !== undefined) el.innerHTML = value;
    });
    root.lang = lang;
    if (root.dataset[lang + 'Title']) document.title = root.dataset[lang + 'Title'];
    if (metaDesc && metaDesc.dataset[lang]) metaDesc.content = metaDesc.dataset[lang];
    if (langBtn) {
      langBtn.textContent = lang === 'it' ? 'EN' : 'IT';
      langBtn.setAttribute('aria-label', lang === 'it' ? 'Switch to English' : "Passa all'italiano");
      langBtn.title = langBtn.getAttribute('aria-label');
    }
    if (typeof setMenu === 'function') setMenu(menuOpen);
  }

  // Phone menu (under 640px): the button exists only when this script runs; without it the links stay in the bar.
  const menuBtn = document.getElementById('menu-toggle');
  const menu = document.getElementById('primary-nav');
  let menuOpen = false;
  const MENU_LABELS = {
    en: { open: 'Open menu', close: 'Close menu' },
    it: { open: 'Apri il menu', close: 'Chiudi il menu' },
  };
  function setMenu(open) {
    menuOpen = open;
    if (!menuBtn || !menu) return;
    const labels = MENU_LABELS[root.lang === 'it' ? 'it' : 'en'];
    menuBtn.setAttribute('aria-expanded', open ? 'true' : 'false');
    menuBtn.setAttribute('aria-label', open ? labels.close : labels.open);
    root.classList.toggle('menu-open', open);
  }

  // ?lang=it / ?lang=en wins, then the saved choice, then the browser language.
  const fromUrl = new URLSearchParams(location.search).get('lang');
  let lang = ['en', 'it'].includes(fromUrl) ? fromUrl
    : saved() || ((navigator.language || 'en').toLowerCase().startsWith('it') ? 'it' : 'en');
  apply(lang);

  if (menuBtn && menu) {
    menuBtn.hidden = false;
    root.classList.add('menu-ready');
    setMenu(false);
    menuBtn.addEventListener('click', () => setMenu(!menuOpen));
    menu.addEventListener('click', (e) => { if (e.target.closest('a')) setMenu(false); });
    document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape' && menuOpen) { setMenu(false); menuBtn.focus(); }
    });
    window.matchMedia('(min-width: 641px)').addEventListener('change', (e) => { if (e.matches) setMenu(false); });
  }

  if (langBtn) {
    langBtn.addEventListener('click', () => {
      lang = lang === 'it' ? 'en' : 'it';
      apply(lang);
      remember(lang);
    });
  }

  const year = document.getElementById('year');
  if (year) year.textContent = new Date().getFullYear();

  // Entrance on scroll with Motion (motion.dev), optional: a short fade and rise.
  // Content is visible by default; it is hidden only while Motion is expected (class
  // motion-ready, removed after 1.5 s at most) and never with reduced motion.
  const M = window.Motion;
  const reduceMotion = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;
  clearTimeout(window.__foReveal);
  if (!M || reduceMotion || !M.animate || !M.inView || !root.classList.contains('motion-ready')) {
    root.classList.remove('motion-ready');
    return;
  }

  const ease = [0.22, 1, 0.36, 1];
  const reveals = Array.prototype.slice.call(document.querySelectorAll('.reveal'));
  // Hand the hidden start state from the class to inline styles, then fade and rise each element as it enters.
  reveals.forEach((el) => { el.style.opacity = '0'; });
  root.classList.remove('motion-ready');
  const showAll = () => reveals.forEach((el) => { el.style.opacity = ''; el.style.transform = ''; });

  // Elements entering together get a small stagger (50 ms, at most six steps).
  let batch = 0;
  let batchTimer = null;
  try {
    reveals.forEach((el) => {
      M.inView(el, () => {
        const delay = Math.min(batch, 5) * 0.05;
        batch += 1;
        clearTimeout(batchTimer);
        batchTimer = setTimeout(() => { batch = 0; }, 120);
        M.animate(el, { opacity: [0, 1], transform: ['translateY(8px)', 'translateY(0px)'] },
          { duration: 0.4, delay: delay, ease: ease });
      }, { amount: 0.2 });
    });
  } catch (e) {
    showAll();
  }
});
