import * as THREE from 'https://cdn.jsdelivr.net/npm/three@0.180.0/build/three.module.js';

const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
let revealObserver;

function initReveal(){
  if(revealObserver) revealObserver.disconnect();
  const items=document.querySelectorAll('.reveal,.stagger');
  if(reducedMotion){items.forEach(x=>x.classList.add('visible'));return;}
  revealObserver=new IntersectionObserver(entries=>{entries.forEach(entry=>{if(!entry.isIntersecting)return;const delay=Number(entry.target.dataset.delay||0);setTimeout(()=>entry.target.classList.add('visible'),delay);revealObserver.unobserve(entry.target);});},{threshold:.08,rootMargin:'0px 0px -30px'});
  items.forEach(x=>revealObserver.observe(x));
}

function initHeader(){
  const header=document.querySelector('[data-header]'); if(!header)return;
  const update=()=>header.classList.toggle('scrolled',window.scrollY>12); update();
  window.onscroll=()=>{update();const b=document.querySelector('[data-back-top]');if(b)b.classList.toggle('visible',window.scrollY>500)};
}

function initMobileMenu(){
  const btn=document.querySelector('[data-menu-button]');const menu=document.querySelector('[data-mobile-menu]');if(!btn||!menu)return;
  btn.onclick=()=>{const open=menu.classList.toggle('open');btn.setAttribute('aria-expanded',String(open));btn.textContent=open?'✕':'☰'};
  menu.querySelectorAll('a').forEach(a=>a.onclick=()=>menu.classList.remove('open'));
}

function initBackToTop(){const b=document.querySelector('[data-back-top]');if(b)b.onclick=()=>window.scrollTo({top:0,behavior:reducedMotion?'auto':'smooth'});}

function initTilt(){if(reducedMotion||window.matchMedia('(pointer: coarse)').matches)return;document.querySelectorAll('.portfolio-project-card,.screen-card,.resource-card').forEach(card=>{card.onpointermove=e=>{const r=card.getBoundingClientRect();const x=(e.clientX-r.left)/r.width-.5;const y=(e.clientY-r.top)/r.height-.5;card.style.transform=`perspective(900px) rotateX(${-y*3}deg) rotateY(${x*4}deg) translateY(-4px)`};card.onpointerleave=()=>card.style.transform='';});}

function initHeroParallax(){if(reducedMotion)return;const profile=document.querySelector('.portfolio-profile');if(!profile)return;const hero=document.querySelector('.portfolio-hero');if(!hero)return;hero.onpointermove=e=>{const r=hero.getBoundingClientRect();const x=(e.clientX-r.left)/r.width-.5;const y=(e.clientY-r.top)/r.height-.5;profile.style.setProperty('--hero-x',`${x*10}px`);profile.style.setProperty('--hero-y',`${y*7}px`)};hero.onpointerleave=()=>{profile.style.setProperty('--hero-x','0px');profile.style.setProperty('--hero-y','0px')};}

function initHero3D(){
  const host=document.getElementById('hero-3d');if(!host||reducedMotion||host.dataset.initialized==='true')return;host.dataset.initialized='true';
  const scene=new THREE.Scene();const camera=new THREE.PerspectiveCamera(42,1,.1,100);camera.position.z=8.2;const renderer=new THREE.WebGLRenderer({alpha:true,antialias:true});renderer.setPixelRatio(Math.min(devicePixelRatio,1.5));renderer.setClearColor(0,0);host.appendChild(renderer.domElement);const group=new THREE.Group();scene.add(group);
  const knot=new THREE.Mesh(new THREE.TorusKnotGeometry(1.7,.035,180,16),new THREE.MeshBasicMaterial({color:0x7da7ff,transparent:true,opacity:.28}));group.add(knot);const ring=new THREE.Mesh(new THREE.TorusGeometry(2.55,.012,8,150),new THREE.MeshBasicMaterial({color:0x8d76ff,transparent:true,opacity:.22}));ring.rotation.x=1.18;ring.rotation.y=.22;group.add(ring);
  const geo=new THREE.BufferGeometry();const count=120;const pos=new Float32Array(count*3);for(let i=0;i<count;i++){const r=3.1+Math.random()*2.3,a=Math.random()*Math.PI*2,b=(Math.random()-.5)*Math.PI;pos[i*3]=Math.cos(a)*Math.cos(b)*r;pos[i*3+1]=Math.sin(b)*r;pos[i*3+2]=Math.sin(a)*Math.cos(b)*r}geo.setAttribute('position',new THREE.BufferAttribute(pos,3));const stars=new THREE.Points(geo,new THREE.PointsMaterial({color:0x9ebdff,size:.025,transparent:true,opacity:.3}));group.add(stars);
  let tx=0,ty=0;host.onpointermove=e=>{const r=host.getBoundingClientRect();tx=((e.clientX-r.left)/r.width-.5)*.3;ty=((e.clientY-r.top)/r.height-.5)*.24};host.onpointerleave=()=>{tx=0;ty=0};const resize=()=>{const w=Math.max(host.clientWidth,1),h=Math.max(host.clientHeight,1);renderer.setSize(w,h,false);camera.aspect=w/h;camera.updateProjectionMatrix()};resize();const ro=new ResizeObserver(resize);ro.observe(host);let frame;const animate=()=>{group.rotation.y+=(tx-group.rotation.y)*.025;group.rotation.x+=(-ty-group.rotation.x)*.025;knot.rotation.z+=.0012;ring.rotation.z-=.0008;stars.rotation.y+=.0003;renderer.render(scene,camera);frame=requestAnimationFrame(animate)};animate();window.addEventListener('pagehide',()=>{cancelAnimationFrame(frame);ro.disconnect();renderer.dispose()},{once:true});
}

function boot(){window.scrollTo(0,0);initReveal();initHeader();initMobileMenu();initBackToTop();initTilt();initHeroParallax();initHero3D();}
boot();document.addEventListener('enhancedload',boot);
