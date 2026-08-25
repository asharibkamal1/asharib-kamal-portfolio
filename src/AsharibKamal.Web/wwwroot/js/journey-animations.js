const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
let journeyObserver;

function initJourneyAnimation() {
  if (journeyObserver) {
    journeyObserver.disconnect();
    journeyObserver = null;
  }

  const section = document.querySelector('.experience-section');
  if (!section) return;

  const rows = [...section.querySelectorAll('.experience-row')];

  if (reducedMotion || !('IntersectionObserver' in window)) {
    section.classList.add('journey-active');
    rows.forEach(row => row.classList.add('step-active'));
    return;
  }

  journeyObserver = new IntersectionObserver(entries => {
    for (const entry of entries) {
      if (!entry.isIntersecting) continue;

      section.classList.add('journey-active');
      rows.forEach((row, index) => {
        window.setTimeout(() => row.classList.add('step-active'), index * 105);
      });

      journeyObserver.disconnect();
      journeyObserver = null;
      break;
    }
  }, { threshold: 0.12, rootMargin: '80px 0px -40px 0px' });

  journeyObserver.observe(section);
}

function bootJourney() {
  window.requestAnimationFrame(() => window.requestAnimationFrame(initJourneyAnimation));
}

if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', bootJourney, { once: true });
} else {
  bootJourney();
}

document.addEventListener('enhancedload', bootJourney);
