/**
 * TruongSach Enhanced JavaScript Framework v2.0
 * Modern UI Components and Interactive Features
 */

class TruongSachFramework {
    constructor() {
        this.init();
    }

    init() {
        this.initToastSystem();
        this.initModalSystem();
        this.initAnimations();
        this.initFormEnhancements();
        this.initImageLightbox();
        this.initSearchFilters();
        this.initWishlist();
        this.initSocialSharing();
        this.initScrollEffects();
        this.initDashboard();
    }

    // Toast Notification System
    initToastSystem() {
        this.toastContainer = this.createToastContainer();
    }

    createToastContainer() {
        const container = document.createElement('div');
        container.id = 'TruongSach-toast-container';
        container.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 1050;
            pointer-events: none;
        `;
        document.body.appendChild(container);
        return container;
    }

    showToast(message, type = 'info', duration = 5000) {
        const toast = document.createElement('div');
        toast.className = `TruongSach-toast TruongSach-toast-${type}`;
        toast.style.pointerEvents = 'auto';
        
        const icon = this.getToastIcon(type);
        toast.innerHTML = `
            <div style="display: flex; align-items: center; gap: 12px;">
                <i class="fas fa-${icon}" style="color: var(--TruongSach-${type});"></i>
                <span style="flex: 1;">${message}</span>
                <button onclick="this.parentElement.parentElement.remove()" 
                        style="background: none; border: none; color: #999; cursor: pointer;">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        `;

        this.toastContainer.appendChild(toast);
        
        // Show animation
        setTimeout(() => toast.classList.add('show'), 100);
        
        // Auto remove
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, duration);
    }

    getToastIcon(type) {
        const icons = {
            success: 'check-circle',
            error: 'exclamation-circle',
            warning: 'exclamation-triangle',
            info: 'info-circle'
        };
        return icons[type] || 'info-circle';
    }

    // Modal System
    initModalSystem() {
        document.addEventListener('click', (e) => {
            if (e.target.matches('[data-TruongSach-modal]')) {
                e.preventDefault();
                const modalId = e.target.getAttribute('data-TruongSach-modal');
                this.showModal(modalId);
            }
            
            if (e.target.matches('.TruongSach-modal') || e.target.matches('[data-modal-close]')) {
                this.hideModal();
            }
        });
    }

    showModal(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.classList.add('show');
            document.body.style.overflow = 'hidden';
        }
    }

    hideModal() {
        const modals = document.querySelectorAll('.TruongSach-modal.show');
        modals.forEach(modal => {
            modal.classList.remove('show');
        });
        document.body.style.overflow = '';
    }

    // Wishlist System
    initWishlist() {
        this.wishlist = JSON.parse(localStorage.getItem('TruongSach-wishlist') || '[]');
        this.updateWishlistUI();
        
        document.addEventListener('click', (e) => {
            if (e.target.matches('[data-wishlist]') || e.target.closest('[data-wishlist]')) {
                e.preventDefault();
                const btn = e.target.matches('[data-wishlist]') ? e.target : e.target.closest('[data-wishlist]');
                const itemId = btn.dataset.wishlist;
                this.toggleWishlist(itemId);
            }
        });
    }

    toggleWishlist(itemId) {
        const index = this.wishlist.indexOf(itemId);
        if (index > -1) {
            this.wishlist.splice(index, 1);
            this.showToast('ÄĂ£ xĂ³a khá»i danh sĂ¡ch yĂªu thĂ­ch', 'info');
        } else {
            this.wishlist.push(itemId);
            this.showToast('ÄĂ£ thĂªm vĂ o danh sĂ¡ch yĂªu thĂ­ch', 'success');
        }
        
        localStorage.setItem('TruongSach-wishlist', JSON.stringify(this.wishlist));
        this.updateWishlistUI();

        // Trigger custom event for header update
        document.dispatchEvent(new CustomEvent('wishlistUpdated'));
    }

    updateWishlistUI() {
        document.querySelectorAll('[data-wishlist]').forEach(btn => {
            const itemId = btn.dataset.wishlist;
            const isInWishlist = this.wishlist.includes(itemId);
            btn.classList.toggle('active', isInWishlist);
            
            const icon = btn.querySelector('i');
            if (icon) {
                icon.className = isInWishlist ? 'fas fa-heart' : 'far fa-heart';
                icon.style.color = isInWishlist ? '#dc3545' : '';
            }
        });
    }

    // Search and Filter System
    initSearchFilters() {
        const searchInputs = document.querySelectorAll('[data-search]');
        searchInputs.forEach(input => {
            input.addEventListener('input', (e) => {
                this.performSearch(e.target.value, e.target.dataset.search);
            });
        });

        // Category filters
        const categoryFilters = document.querySelectorAll('[data-filter-category]');
        categoryFilters.forEach(filter => {
            filter.addEventListener('click', (e) => {
                e.preventDefault();
                const category = e.target.dataset.filterCategory;
                this.filterByCategory(category);
            });
        });
    }

    performSearch(query, target) {
        const items = document.querySelectorAll(`[data-searchable="${target}"]`);
        items.forEach(item => {
            const text = item.textContent.toLowerCase();
            const match = text.includes(query.toLowerCase());
            item.style.display = match ? '' : 'none';
        });
    }

    filterByCategory(category) {
        const items = document.querySelectorAll('[data-category]');
        items.forEach(item => {
            const itemCategory = item.dataset.category;
            const match = category === 'all' || itemCategory === category;
            item.style.display = match ? '' : 'none';
        });
    }

    // Image Lightbox
    initImageLightbox() {
        document.addEventListener('click', (e) => {
            if (e.target.matches('[data-lightbox]')) {
                e.preventDefault();
                this.openLightbox(e.target.src, e.target.alt);
            }
        });
    }

    openLightbox(src, alt) {
        const lightbox = document.createElement('div');
        lightbox.className = 'TruongSach-lightbox';
        lightbox.innerHTML = `
            <div class="TruongSach-lightbox-content">
                <img src="${src}" alt="${alt}" style="max-width: 90vw; max-height: 90vh; border-radius: 8px;">
                <button class="TruongSach-lightbox-close" onclick="this.parentElement.parentElement.remove()">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        `;
        
        lightbox.style.cssText = `
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(0,0,0,0.9); z-index: 1070;
            display: flex; align-items: center; justify-content: center;
            backdrop-filter: blur(10px);
        `;
        
        document.body.appendChild(lightbox);
        
        lightbox.addEventListener('click', (e) => {
            if (e.target === lightbox) lightbox.remove();
        });
    }

    // Social Sharing
    initSocialSharing() {
        document.addEventListener('click', (e) => {
            if (e.target.matches('[data-share]') || e.target.closest('[data-share]')) {
                e.preventDefault();
                const btn = e.target.matches('[data-share]') ? e.target : e.target.closest('[data-share]');
                const platform = btn.dataset.share;
                const url = btn.dataset.url || window.location.href;
                const title = btn.dataset.title || document.title;
                this.shareToSocial(platform, url, title);
            }
        });
    }

    shareToSocial(platform, url, title) {
        const shareUrls = {
            facebook: `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(url)}`,
            twitter: `https://twitter.com/intent/tweet?url=${encodeURIComponent(url)}&text=${encodeURIComponent(title)}`,
            linkedin: `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(url)}`,
            whatsapp: `https://wa.me/?text=${encodeURIComponent(title + ' ' + url)}`
        };
        
        if (shareUrls[platform]) {
            window.open(shareUrls[platform], '_blank', 'width=600,height=400');
        }
    }

    // Animation System
    initAnimations() {
        this.observeElements();
    }

    observeElements() {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('animate');
                }
            });
        }, { threshold: 0.1 });

        document.querySelectorAll('[data-animate]').forEach(el => {
            observer.observe(el);
        });
    }

    // Form Enhancements
    initFormEnhancements() {
        this.initFormValidation();
        this.initFileUpload();
    }

    initFormValidation() {
        document.addEventListener('submit', (e) => {
            const form = e.target;
            if (form.hasAttribute('data-validate')) {
                if (!this.validateForm(form)) {
                    e.preventDefault();
                }
            }
        });
    }

    validateForm(form) {
        let isValid = true;
        const inputs = form.querySelectorAll('[required]');
        
        inputs.forEach(input => {
            if (!input.value.trim()) {
                this.showFieldError(input, 'TrÆ°á»ng nĂ y lĂ  báº¯t buá»™c');
                isValid = false;
            } else {
                this.clearFieldError(input);
            }
        });
        
        return isValid;
    }

    showFieldError(input, message) {
        this.clearFieldError(input);
        const error = document.createElement('div');
        error.className = 'TruongSach-field-error';
        error.textContent = message;
        error.style.cssText = `
            color: var(--TruongSach-danger);
            font-size: 0.875rem;
            margin-top: 4px;
        `;
        input.parentNode.appendChild(error);
        input.style.borderColor = 'var(--TruongSach-danger)';
    }

    clearFieldError(input) {
        const error = input.parentNode.querySelector('.TruongSach-field-error');
        if (error) error.remove();
        input.style.borderColor = '';
    }

    // Scroll Effects
    initScrollEffects() {
        let ticking = false;
        
        window.addEventListener('scroll', () => {
            if (!ticking) {
                requestAnimationFrame(() => {
                    this.updateScrollEffects();
                    ticking = false;
                });
                ticking = true;
            }
        });
    }

    updateScrollEffects() {
        const scrolled = window.pageYOffset;
        const parallaxElements = document.querySelectorAll('[data-parallax]');
        
        parallaxElements.forEach(element => {
            const speed = element.dataset.parallax || 0.5;
            const yPos = -(scrolled * speed);
            element.style.transform = `translateY(${yPos}px)`;
        });
    }

    // Dashboard Analytics
    initDashboard() {
        this.initCounters();
        this.initCharts();
    }

    initCounters() {
        const counters = document.querySelectorAll('[data-counter]');
        counters.forEach(counter => {
            const target = parseInt(counter.dataset.counter);
            const duration = parseInt(counter.dataset.duration) || 2000;
            this.animateCounter(counter, target, duration);
        });
    }

    animateCounter(element, target, duration) {
        const start = 0;
        const increment = target / (duration / 16);
        let current = start;
        
        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                current = target;
                clearInterval(timer);
            }
            element.textContent = Math.floor(current).toLocaleString();
        }, 16);
    }

    initCharts() {
        // Placeholder for chart initialization
        // Can be extended with Chart.js or other charting libraries
    }

    initFileUpload() {
        const fileInputs = document.querySelectorAll('input[type="file"][data-preview]');
        fileInputs.forEach(input => {
            input.addEventListener('change', (e) => {
                this.handleFilePreview(e.target);
            });
        });
    }

    handleFilePreview(input) {
        const file = input.files[0];
        if (file && file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = (e) => {
                const previewId = input.dataset.preview;
                const preview = document.getElementById(previewId);
                if (preview) {
                    preview.src = e.target.result;
                    preview.style.display = 'block';
                }
            };
            reader.readAsDataURL(file);
        }
    }
}

// Initialize TruongSach Framework
document.addEventListener('DOMContentLoaded', () => {
    window.TruongSach = new TruongSachFramework();
});

// Export for module usage
if (typeof module !== 'undefined' && module.exports) {
    module.exports = TruongSachFramework;
}