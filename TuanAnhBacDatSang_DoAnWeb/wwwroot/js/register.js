
// Navbar scroll effect
window.addEventListener('scroll', function () {
    const navbar = document.querySelector('.TruongSach-navbar');
    if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
    } else {
        navbar.classList.remove('scrolled');
    }
});

// Smooth scroll for navigation links
document.querySelectorAll('.TruongSach-nav-link').forEach(link => {
    link.addEventListener('click', function (e) {
        const href = this.getAttribute('href');
        if (href && href.startsWith('#')) {
            e.preventDefault();
            const target = document.querySelector(href);
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        }
    });
});

// Mobile menu animation
const navbarToggler = document.querySelector('.navbar-toggler');
const navbarCollapse = document.querySelector('.navbar-collapse');

if (navbarToggler) {
    navbarToggler.addEventListener('click', function () {
        this.classList.toggle('active');
    });
}


document.addEventListener('DOMContentLoaded', function () {
    const floatingNav = document.getElementById('floatingNav');
    const navbar = document.querySelector('.TruongSach-navbar');
    let lastScrollTop = 0;
    let scrollTimeout;

    // Show floating nav after scroll
    function handleScroll() {
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        // Clear previous timeout
        clearTimeout(scrollTimeout);

        // Show floating nav when scrolling down past 200px
        if (scrollTop > 200) {
            floatingNav.classList.add('show');

            // Auto hide after 3 seconds of no scrolling (desktop only)
            if (window.innerWidth >= 992) {
                scrollTimeout = setTimeout(() => {
                    floatingNav.classList.remove('show');
                }, 3000);
            }
        } else {
            floatingNav.classList.remove('show');
        }

        // Scrolled class is handled by the main navigation script

        lastScrollTop = scrollTop;
    }

    // Throttled scroll handler
    let ticking = false;
    function requestTick() {
        if (!ticking) {
            requestAnimationFrame(handleScroll);
            ticking = true;
            setTimeout(() => { ticking = false; }, 10);
        }
    }

    window.addEventListener('scroll', requestTick);

    // Show floating nav on hover (desktop)
    if (window.innerWidth >= 992) {
        floatingNav.addEventListener('mouseenter', () => {
            clearTimeout(scrollTimeout);
            floatingNav.classList.add('show');
        });

        floatingNav.addEventListener('mouseleave', () => {
            if (window.pageYOffset > 200) {
                scrollTimeout = setTimeout(() => {
                    floatingNav.classList.remove('show');
                }, 1000);
            }
        });
    }

    // Update wishlist count
    function updateWishlistCount() {
        const wishlistCount = localStorage.getItem('wishlistCount') || '0';
        const headerWishlist = document.getElementById('header-wishlist-count');
        const floatingWishlist = document.getElementById('floating-wishlist-count');

        if (headerWishlist) headerWishlist.textContent = wishlistCount;
        if (floatingWishlist) floatingWishlist.textContent = wishlistCount;

        // Hide badge if count is 0
        if (wishlistCount === '0') {
            if (headerWishlist) headerWishlist.style.display = 'none';
            if (floatingWishlist) floatingWishlist.style.display = 'none';
        } else {
            if (headerWishlist) headerWishlist.style.display = 'flex';
            if (floatingWishlist) floatingWishlist.style.display = 'flex';
        }
    }

    // Initialize wishlist count
    updateWishlistCount();

    // Listen for wishlist updates
    window.addEventListener('storage', updateWishlistCount);
    window.addEventListener('wishlistUpdated', updateWishlistCount);
});




document.addEventListener('DOMContentLoaded', function () {
    const navbar = document.querySelector('.TruongSach-navbar');
    const navbarToggler = document.querySelector('.TruongSach-toggler');
    const navbarCollapse = document.querySelector('.navbar-collapse');

    // Enhanced scroll effects with bottom positioning
    let lastScrollTop = 0;
    let scrollDirection = 'up';
    let isScrolling = false;
    let scrollTimer = null;
    const body = document.body;

    // Debounce scroll events
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    function handleScroll() {
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        console.log('Scroll position:', scrollTop); // Debug

        // Determine scroll direction
        if (scrollTop > lastScrollTop) {
            scrollDirection = 'down';
        } else {
            scrollDirection = 'up';
        }

        // Clear existing timer
        if (scrollTimer) {
            clearTimeout(scrollTimer);
        }

        isScrolling = true;

        // Add scrolled class
        if (scrollTop > 100) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }

        // Handle navbar positioning based on scroll with improved logic
        if (scrollTop > 80) {
            console.log('Moving navbar to bottom'); // Debug
            // Move navbar to bottom when scrolling past threshold
            navbar.classList.remove('navbar-hidden', 'fixed-top');
            navbar.classList.add('navbar-bottom');
            body.classList.add('navbar-at-bottom');

            // Add smooth transition class
            navbar.style.transition = 'all 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
        } else {
            console.log('Moving navbar to top'); // Debug
            // Return to top when near top of page
            navbar.classList.remove('navbar-bottom', 'navbar-hidden');
            navbar.classList.add('fixed-top');
            body.classList.remove('navbar-at-bottom');

            // Add smooth transition class
            navbar.style.transition = 'all 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
        }

        // Set timer to detect when scrolling stops
        scrollTimer = setTimeout(() => {
            isScrolling = false;

            // Ensure navbar stays visible at bottom
            if (scrollTop > 100 && navbar.classList.contains('navbar-bottom')) {
                navbar.classList.remove('navbar-hidden');
            }
        }, 150);

        lastScrollTop = scrollTop;
    }

    // Use debounced scroll handler for better performance
    const debouncedHandleScroll = debounce(handleScroll, 10);

    window.addEventListener('scroll', debouncedHandleScroll, { passive: true });

    // Handle touch events for mobile swipe
    let touchStartY = 0;
    let touchEndY = 0;

    document.addEventListener('touchstart', function (e) {
        touchStartY = e.changedTouches[0].screenY;
    }, { passive: true });

    document.addEventListener('touchend', function (e) {
        touchEndY = e.changedTouches[0].screenY;
        handleSwipe();
    }, { passive: true });

    function handleSwipe() {
        const swipeThreshold = 40;
        const diff = touchStartY - touchEndY;

        if (Math.abs(diff) > swipeThreshold) {
            if (diff > 0) {
                // Swiping up - move navbar to bottom
                if (window.pageYOffset > 50) {
                    navbar.classList.remove('fixed-top');
                    navbar.classList.add('navbar-bottom');
                    body.classList.add('navbar-at-bottom');
                    navbar.style.transition = 'all 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
                }
            } else {
                // Swiping down - return navbar to top if near top
                if (window.pageYOffset < 80) {
                    navbar.classList.remove('navbar-bottom');
                    navbar.classList.add('fixed-top');
                    body.classList.remove('navbar-at-bottom');
                    navbar.style.transition = 'all 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
                }
            }
        }
    }

    // Initialize navbar position on page load
    if (window.pageYOffset > 80) {
        navbar.classList.remove('fixed-top');
        navbar.classList.add('navbar-bottom');
        body.classList.add('navbar-at-bottom');
    } else {
        navbar.classList.add('fixed-top');
        navbar.classList.remove('navbar-bottom');
        body.classList.remove('navbar-at-bottom');
    }

    // Add smooth scroll behavior for better UX
    document.documentElement.style.scrollBehavior = 'smooth';

    // Highlight active menu item in bottom navbar
    function highlightActiveMenuItem() {
        const currentPath = window.location.pathname;
        const navLinks = document.querySelectorAll('.TruongSach-navbar.navbar-bottom .bottom-nav-item .TruongSach-nav-link');

        navLinks.forEach(link => {
            link.classList.remove('active');
            const href = link.getAttribute('href');

            if (href && (currentPath === href || currentPath.startsWith(href + '/') ||
                (href.includes('/') && currentPath.includes(href.split('/').pop())))) {
                link.classList.add('active');
            }
        });
    }

    // Call highlight function when navbar moves to bottom
    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                if (navbar.classList.contains('navbar-bottom')) {
                    setTimeout(highlightActiveMenuItem, 100);
                }
            }
        });
    });

    observer.observe(navbar, { attributes: true });

    // Initial highlight
    if (navbar.classList.contains('navbar-bottom')) {
        highlightActiveMenuItem();
    }

    // Add ripple effect to bottom nav items
    function createRipple(event) {
        const button = event.currentTarget;
        const circle = document.createElement('span');
        const diameter = Math.max(button.clientWidth, button.clientHeight);
        const radius = diameter / 2;

        circle.style.width = circle.style.height = `${diameter}px`;
        circle.style.left = `${event.clientX - button.offsetLeft - radius}px`;
        circle.style.top = `${event.clientY - button.offsetTop - radius}px`;
        circle.classList.add('ripple');

        const ripple = button.getElementsByClassName('ripple')[0];
        if (ripple) {
            ripple.remove();
        }

        button.appendChild(circle);
    }

    // Add ripple effect to all bottom nav links
    document.addEventListener('click', function (e) {
        if (e.target.closest('.TruongSach-navbar.navbar-bottom .bottom-nav-item .TruongSach-nav-link')) {
            createRipple(e);
        }
    });

    // Mobile menu close on link click
    const navLinks = document.querySelectorAll('.TruongSach-nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function () {
            if (window.innerWidth < 992) {
                const collapse = new bootstrap.Collapse(navbarCollapse, {
                    hide: true
                });
            }
        });
    });

    // Enhanced mobile menu animation
    navbarCollapse.addEventListener('show.bs.collapse', function () {
        navbarToggler.setAttribute('aria-expanded', 'true');
    });

    navbarCollapse.addEventListener('hide.bs.collapse', function () {
        navbarToggler.setAttribute('aria-expanded', 'false');
    });

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Update wishlist count in header (only for logged in users)
    function updateWishlistCount() {
    }

    // Initial update (only for logged in users)

    // Show logout message if present
});
// Password toggle functionality
function togglePassword(inputId) {
    const input = document.getElementById(inputId);
    const eye = document.getElementById(inputId + '-eye');

    if (input.type === 'password') {
        input.type = 'text';
        eye.className = 'fas fa-eye-slash';
    } else {
        input.type = 'password';
        eye.className = 'fas fa-eye';
    }
}

// Password strength checker
function checkPasswordStrength(password) {
    let strength = 0;
    let feedback = '';

    if (password.length >= 8) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;

    switch (strength) {
        case 0:
        case 1:
            feedback = '<span class="strength-weak">Yếu</span>';
            break;
        case 2:
        case 3:
            feedback = '<span class="strength-medium">Trung bình</span>';
            break;
        case 4:
        case 5:
            feedback = '<span class="strength-strong">Mạnh</span>';
            break;
    }

    return feedback;
}

// Password match checker
function checkPasswordMatch(password, confirmPassword) {
    if (confirmPassword === '') return '';

    if (password === confirmPassword) {
        return '<span class="match-success"><i class="fas fa-check"></i> Mật khẩu khớp</span>';
    } else {
        return '<span class="match-error"><i class="fas fa-times"></i> Mật khẩu không khớp</span>';
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('register-form');
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirm_password');
    const strengthDiv = document.getElementById('password-strength');
    const matchDiv = document.getElementById('password-match');

    // Password strength checking
    passwordInput.addEventListener('input', function () {
        const strength = checkPasswordStrength(this.value);
        strengthDiv.innerHTML = strength;

        // Also check match if confirm password has value
        if (confirmPasswordInput.value) {
            const match = checkPasswordMatch(this.value, confirmPasswordInput.value);
            matchDiv.innerHTML = match;
        }
    });

    // Password match checking
    confirmPasswordInput.addEventListener('input', function () {
        const match = checkPasswordMatch(passwordInput.value, this.value);
        matchDiv.innerHTML = match;
    });

    // Form submission
    form.addEventListener('submit', function (e) {
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;
        const terms = document.getElementById('terms').checked;

        if (password !== confirmPassword) {
            e.preventDefault();
            if (window.TruongSach) {
                TruongSach.showToast('Mật khẩu xác nhận không khớp', 'error');
            }
            return false;
        }

        if (!terms) {
            e.preventDefault();
            if (window.TruongSach) {
                TruongSach.showToast('Vui lòng đồng ý với điều khoản sử dụng', 'error');
            }
            return false;
        }

        // Show loading state
        const submitBtn = form.querySelector('button[type="submit"]');
        const originalText = submitBtn.innerHTML;
        submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang đăng ký...';
        submitBtn.disabled = true;

        // Re-enable button after 5 seconds (in case of error)
        setTimeout(() => {
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;
        }, 5000);
    });

    // Auto-focus first input
    const firstInput = form.querySelector('input');
    if (firstInput) {
        firstInput.focus();
    }
});