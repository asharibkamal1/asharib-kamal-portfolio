import * as THREE from 'https://cdn.jsdelivr.net/npm/three@0.180.0/build/three.module.js';

const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

function initReveal() {
  const items = document.querySelectorAll('.reveal');
  if (reducedMotion) {
    items.forEach(x => x.classList.add('visible'));
    return;
  }

  const observer = new IntersectionObserver(entries => {
    entries.forEach(entry => {
      if (!entry.isIntersecting) return;
      const delay = Number(entry.target.dataset.delay || 0);
      window.setTimeout(() => entry.target.classList.add('visible'), delay);
      observer.unobserve(entry.target);
    });
  }, { threshold: 0.12 });

  items.forEach(x => observer.observe(x));
}

function initHeader() {
  const header = document.querySelector('[data-header]');
  if (!header) return;
  const update = () => header.classList.toggle('scrolled', window.scrollY > 16);
  update();
  window.addEventListener('scroll', update, { passive: true });
}

function initHero3D() {
  const host = document.getElementById('hero-3d');
  if (!host || reducedMotion || host.dataset.initialized === 'true') return;
  host.dataset.initialized = 'true';

  const scene = new THREE.Scene();
  const camera = new THREE.PerspectiveCamera(42, 1, 0.1, 100);
  camera.position.z = 8.2;

  const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
  renderer.setPixelRatio(Math.min(window.devicePixelRatio, 1.6));
  renderer.setClearColor(0x000000, 0);
  host.appendChild(renderer.domElement);

  const group = new THREE.Group();
  scene.add(group);

  const knot = new THREE.Mesh(
    new THREE.TorusKnotGeometry(1.7, 0.035, 220, 18),
    new THREE.MeshBasicMaterial({ color: 0x7da7ff, transparent: true, opacity: 0.34 })
  );
  group.add(knot);

  const ring = new THREE.Mesh(
    new THREE.TorusGeometry(2.55, 0.012, 8, 180),
    new THREE.MeshBasicMaterial({ color: 0x8d76ff, transparent: true, opacity: 0.26 })
  );
  ring.rotation.x = 1.18;
  ring.rotation.y = 0.22;
  group.add(ring);

  const nodeGeometry = new THREE.SphereGeometry(0.055, 18, 18);
  const points = [
    [-2.4, 1.25, .3], [2.35, 1.4, -.2], [2.55, -1.35, .2],
    [-2.2, -1.45, -.1], [0, 2.45, .1], [0, -2.5, .25]
  ];
  points.forEach((p, i) => {
    const node = new THREE.Mesh(
      nodeGeometry,
      new THREE.MeshBasicMaterial({ color: i % 2 ? 0x9c82ff : 0x83b0ff })
    );
    node.position.set(...p);
    group.add(node);
  });

  const starGeometry = new THREE.BufferGeometry();
  const starCount = 180;
  const positions = new Float32Array(starCount * 3);
  for (let i = 0; i < starCount; i++) {
    const r = 3.2 + Math.random() * 2.8;
    const a = Math.random() * Math.PI * 2;
    const b = (Math.random() - .5) * Math.PI;
    positions[i * 3] = Math.cos(a) * Math.cos(b) * r;
    positions[i * 3 + 1] = Math.sin(b) * r;
    positions[i * 3 + 2] = Math.sin(a) * Math.cos(b) * r;
  }
  starGeometry.setAttribute('position', new THREE.BufferAttribute(positions, 3));
  const stars = new THREE.Points(starGeometry, new THREE.PointsMaterial({ color: 0x9ebdff, size: 0.025, transparent: true, opacity: 0.35 }));
  group.add(stars);

  let targetX = 0;
  let targetY = 0;
  const onPointer = e => {
    const rect = host.getBoundingClientRect();
    targetX = ((e.clientX - rect.left) / rect.width - .5) * .32;
    targetY = ((e.clientY - rect.top) / rect.height - .5) * .26;
  };
  host.addEventListener('pointermove', onPointer);
  host.addEventListener('pointerleave', () => { targetX = 0; targetY = 0; });

  const resize = () => {
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
  const animate = () => {
    group.rotation.y += (targetX - group.rotation.y) * .025;
    group.rotation.x += (-targetY - group.rotation.x) * .025;
    knot.rotation.z += .0015;
    ring.rotation.z -= .001;
    stars.rotation.y += .00035;
    renderer.render(scene, camera);
    frame = requestAnimationFrame(animate);
  };
  animate();

  window.addEventListener('pagehide', () => {
    cancelAnimationFrame(frame);
    ro.disconnect();
    renderer.dispose();
  }, { once: true });
}

function boot() {
  initReveal();
  initHeader();
  initHero3D();
}

boot();
document.addEventListener('enhancedload', () => {
  window.scrollTo({ top: 0, left: 0, behavior: 'instant' });
  boot();
});
