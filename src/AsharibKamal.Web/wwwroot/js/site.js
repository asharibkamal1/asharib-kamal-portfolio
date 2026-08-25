const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
let revealObserver;
let hero3DState;

// Progressive enhancement: content is visible by default in CSS.
// We only opt into hidden/reveal states after JavaScript has loaded successfully.
document.documentElement.classList.add('js-animations');

function initReveal() {
  if (revealObserver) {
    revealObserver.disconnect();
    revealObserver = null;
  }

  const items = document.querySelectorAll('.reveal,.stagger');
  if (!items.length) return;

  if (reducedMotion || !('IntersectionObserver' in window)) {
    items.forEach(x => x.classList.add('visible'));
    return;
  }

  // Anything already in/near the viewport becomes visible immediately.
  items.forEach(item => {
    const rect = item.getBoundingClientRect();
    if (rect.top < window.innerHeight + 80 && rect.bottom > -80) {
      item.classList.add('visible');
    }
  });

  revealObserver = new IntersectionObserver(entries => {
    entries.forEach(entry => {
      if (!entry.isIntersecting) return;
      const element = entry.target;
      const delay = Number(element.dataset.delay || 0);
      window.setTimeout(() => element.classList.add('visible'), delay);
      revealObserver?.unobserve(element);
    });
  }, { threshold: 0.04, rootMargin: '120px 0px 120px 0px' });

  items.forEach(item => {
    if (!item.classList.contains('visible')) revealObserver.observe(item);
  });
}

function initHeader() {
  const header = document.querySelector('[data-header]');
  const backTop = document.querySelector('[data-back-top]');
  if (!header) return;

  const update = () => {
    header.classList.toggle('scrolled', window.scrollY > 12);
    if (backTop) backTop.classList.toggle('visible', window.scrollY > 500);
  };

  update();
  window.onscroll = update;
}

function initMobileMenu() {
  const btn = document.querySelector('[data-menu-button]');
  const menu = document.querySelector('[data-mobile-menu]');
  if (!btn || !menu) return;

  btn.onclick = () => {
    const open = menu.classList.toggle('open');
    btn.setAttribute('aria-expanded', String(open));
    btn.textContent = open ? '✕' : '☰';
  };

  menu.querySelectorAll('a').forEach(a => {
    a.onclick = () => {
      menu.classList.remove('open');
      btn.setAttribute('aria-expanded', 'false');
      btn.textContent = '☰';
    };
  });
}

function initBackToTop() {
  const button = document.querySelector('[data-back-top]');
  if (!button) return;
  button.onclick = () => window.scrollTo({ top: 0, behavior: reducedMotion ? 'auto' : 'smooth' });
}

function initTilt() {
  if (reducedMotion || window.matchMedia('(pointer: coarse)').matches) return;

  document.querySelectorAll('.portfolio-project-card,.screen-card,.resource-card').forEach(card => {
    card.onpointermove = e => {
      const rect = card.getBoundingClientRect();
      const x = (e.clientX - rect.left) / rect.width - .5;
      const y = (e.clientY - rect.top) / rect.height - .5;
      card.style.transform = `perspective(900px) rotateX(${-y * 3}deg) rotateY(${x * 4}deg) translateY(-4px)`;
    };
    card.onpointerleave = () => card.style.transform = '';
  });
}

function initHeroParallax() {
  if (reducedMotion) return;
  const profile = document.querySelector('.portfolio-profile');
  const hero = document.querySelector('.portfolio-hero');
  if (!profile || !hero) return;

  hero.onpointermove = e => {
    const rect = hero.getBoundingClientRect();
    const x = (e.clientX - rect.left) / rect.width - .5;
    const y = (e.clientY - rect.top) / rect.height - .5;
    profile.style.setProperty('--hero-x', `${x * 10}px`);
    profile.style.setProperty('--hero-y', `${y * 7}px`);
  };
  hero.onpointerleave = () => {
    profile.style.setProperty('--hero-x', '0px');
    profile.style.setProperty('--hero-y', '0px');
  };
}

async function initHero3D() {
  const host = document.getElementById('hero-3d');
  if (!host || reducedMotion || host.dataset.initialized === 'true') return;

  // Three.js must never block the rest of the website. Load it only when this
  // optional canvas actually exists and fail gracefully if the CDN is unavailable.
  try {
    const THREE = await import('https://cdn.jsdelivr.net/npm/three@0.180.0/build/three.module.js');
    if (!document.body.contains(host)) return;

    host.dataset.initialized = 'true';
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(42, 1, .1, 100);
    camera.position.z = 8.2;

    const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
    renderer.setPixelRatio(Math.min(window.devicePixelRatio, 1.5));
    renderer.setClearColor(0, 0);
    host.appendChild(renderer.domElement);

    const group = new THREE.Group();
    scene.add(group);

    const knot = new THREE.Mesh(
      new THREE.TorusKnotGeometry(1.7, .035, 180, 16),
      new THREE.MeshBasicMaterial({ color: 0x7da7ff, transparent: true, opacity: .28 })
    );
    group.add(knot);

    const ring = new THREE.Mesh(
      new THREE.TorusGeometry(2.55, .012, 8, 150),
      new THREE.MeshBasicMaterial({ color: 0x8d76ff, transparent: true, opacity: .22 })
    );
    ring.rotation.x = 1.18;
    ring.rotation.y = .22;
    group.add(ring);

    const geo = new THREE.BufferGeometry();
    const count = 120;
    const pos = new Float32Array(count * 3);
    for (let i = 0; i < count; i++) {
      const radius = 3.1 + Math.random() * 2.3;
      const a = Math.random() * Math.PI * 2;
      const b = (Math.random() - .5) * Math.PI;
      pos[i * 3] = Math.cos(a) * Math.cos(b) * radius;
      pos[i * 3 + 1] = Math.sin(b) * radius;
      pos[i * 3 + 2] = Math.sin(a) * Math.cos(b) * radius;
    }
    geo.setAttribute('position', new THREE.BufferAttribute(pos, 3));
    const stars = new THREE.Points(
      geo,
      new THREE.PointsMaterial({ color: 0x9ebdff, size: .025, transparent: true, opacity: .3 })
    );
    group.add(stars);

    let tx = 0;
    let ty = 0;
    host.onpointermove = e => {
      const rect = host.getBoundingClientRect();
      tx = ((e.clientX - rect.left) / rect.width - .5) * .3;
      ty = ((e.clientY - rect.top) / rect.height - .5) * .24;
    };
    host.onpointerleave = () => { tx = 0; ty = 0; };

    const resize = () => {
      if (!document.body.contains(host)) return;
      const w = Math.max(host.clientWidth, 1);
      const h = Math.max(host.clientHeight, 1);
      renderer.setSize(w, h, false);
      camera.aspect = w / h;
      camera.updateProjectionMatrix();
    };
    resize();
    const ro = new ResizeObserver(resize);
    ro.observe(host);

    let frame;
    let disposed = false;
    const animate = () => {
      if (disposed || !document.body.contains(host)) {
        disposed = true;
        cancelAnimationFrame(frame);
        ro.disconnect();
        renderer.dispose();
        hero3DState = null;
        return;
      }
      group.rotation.y += (tx - group.rotation.y) * .025;
      group.rotation.x += (-ty - group.rotation.x) * .025;
      knot.rotation.z += .0012;
      ring.rotation.z -= .0008;
      stars.rotation.y += .0003;
      renderer.render(scene, camera);
      frame = requestAnimationFrame(animate);
    };
    animate();
    hero3DState = { renderer, ro };
  } catch (error) {
    console.warn('Optional 3D scene could not be loaded. Portfolio content remains available.', error);
    host.dataset.initialized = 'failed';
  }
}

function boot({ resetScroll = false } = {}) {
  if (resetScroll) window.scrollTo({ top: 0, left: 0, behavior: 'auto' });
  initReveal();
  initHeader();
  initMobileMenu();
  initBackToTop();
  initTilt();
  initHeroParallax();
  void initHero3D();
}

function bootWhenReady(options) {
  // Allow Blazor/enhanced-navigation DOM replacement to finish before querying it.
  window.requestAnimationFrame(() => window.requestAnimationFrame(() => boot(options)));
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', () => bootWhenReady(), { once: true });
} else {
  bootWhenReady();
}

document.addEventListener('enhancedload', () => bootWhenReady({ resetScroll: true }));
