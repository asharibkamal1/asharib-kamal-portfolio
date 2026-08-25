const prefersReducedMotion=window.matchMedia('(prefers-reduced-motion: reduce)').matches;
let scrollFrame=0;

function clamp(value,min,max){return Math.min(Math.max(value,min),max)}

function sectionProgress(element){
  const rect=element.getBoundingClientRect();
  const vh=Math.max(window.innerHeight,1);
  return clamp((vh-rect.top)/(vh+rect.height),0,1);
}

function initHomeScrollMotion(){
  const hero=document.querySelector('.portfolio-hero');
  if(!hero||prefersReducedMotion)return;

  document.documentElement.classList.add('home-scroll-motion');
  const sections=[...document.querySelectorAll('.portfolio-section')];
  const projectCards=[...document.querySelectorAll('.portfolio-project-card')];
  const experienceRows=[...document.querySelectorAll('.experience-row')];
  const skills=[...document.querySelectorAll('.skill-cluster')];
  const articleCards=[...document.querySelectorAll('.portfolio-article-card')];
  const depthSections=[...document.querySelectorAll('.home-newsletter-strip,.resources-section,.product-section,.final-cta-section')];

  const render=()=>{
    scrollFrame=0;
    const doc=document.documentElement;
    const maxScroll=Math.max(doc.scrollHeight-window.innerHeight,1);
    doc.style.setProperty('--scroll-progress',String(clamp(window.scrollY/maxScroll,0,1)));

    sections.forEach(section=>{
      const p=sectionProgress(section);
      const active=p>0&&p<1;
      section.dataset.scrollActive=String(active);
      section.style.setProperty('--ambient-y',`${(p-.5)*34}px`);
      section.style.setProperty('--section-title-y',`${(0.5-Math.abs(p-.5))*-5}px`);
    });

    projectCards.forEach((card,index)=>{
      const p=sectionProgress(card);
      const wave=Math.sin(p*Math.PI);
      card.style.setProperty('--card-scroll-y',`${(1-wave)*Math.min(8+index*1.3,13)}px`);
    });

    let closest=null;
    let closestDistance=Infinity;
    const viewportCenter=window.innerHeight*.5;
    experienceRows.forEach(row=>{
      const rect=row.getBoundingClientRect();
      const center=rect.top+rect.height/2;
      const distance=Math.abs(center-viewportCenter);
      if(rect.bottom>0&&rect.top<window.innerHeight&&distance<closestDistance){closest=row;closestDistance=distance}
    });
    experienceRows.forEach(row=>row.classList.toggle('scroll-current',row===closest));

    skills.forEach((card,index)=>{
      const p=sectionProgress(card);
      const shift=(p-.5)*(index%2===0?-10:10);
      card.style.setProperty('--skill-shift',`${clamp(shift,-5,5)}px`);
    });

    articleCards.forEach((card,index)=>{
      const p=sectionProgress(card);
      const wave=Math.sin(p*Math.PI);
      card.style.setProperty('--article-scale',String(.992+wave*.008));
      card.style.setProperty('--article-y',`${(1-wave)*(5+index)}px`);
    });

    depthSections.forEach(section=>{
      const p=sectionProgress(section);
      section.style.setProperty('--section-depth-y',`${(p-.5)*-10}px`);
    });
  };

  const requestRender=()=>{if(scrollFrame)return;scrollFrame=requestAnimationFrame(render)};
  window.addEventListener('scroll',requestRender,{passive:true});
  window.addEventListener('resize',requestRender,{passive:true});
  render();
}

function bootScrollMotion(){requestAnimationFrame(()=>requestAnimationFrame(initHomeScrollMotion))}

if(document.readyState==='loading')document.addEventListener('DOMContentLoaded',bootScrollMotion,{once:true});else bootScrollMotion();
document.addEventListener('enhancedload',bootScrollMotion);
