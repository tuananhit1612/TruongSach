
// Modern Donate Page JavaScript
    document.addEventListener('DOMContentLoaded', function() {
        // Initialize animations
        initScrollAnimations();

    // Initialize form interactions
    initFormInteractions();

    // Initialize event handlers
    initEventHandlers();
});

    // Scroll Functions
    function scrollToDonateForm() {
        document.getElementById('donate-form').scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });
}

    function scrollToEvents() {
        document.getElementById('events-section').scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });
}

    function initScrollAnimations() {
    const observerOptions = {
        threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    document.querySelectorAll('[data-animate]').forEach(el => {
        observer.observe(el);
    });
}

    function initFormInteractions() {
    // Handle preset amount selection
    const amountRadios = document.querySelectorAll('input[name="amount"]');
    const customAmountInput = document.getElementById('custom_amount');
    
    amountRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            if (this.checked) {
                customAmountInput.value = '';
                updateAmountDisplay();
            }
        });
    });

    customAmountInput.addEventListener('input', function() {
        if (this.value) {
        amountRadios.forEach(radio => radio.checked = false);
        }
    updateAmountDisplay();
    });

    // Handle payment method selection
    const paymentMethods = document.querySelectorAll('input[name="payment_method"]');
    const bankInfo = document.getElementById('bank-info');
    const cashInfo = document.getElementById('cash-info');
    
    paymentMethods.forEach(method => {
        method.addEventListener('change', function () {
            // Hide all info sections
            if (bankInfo) bankInfo.style.display = 'none';
            if (cashInfo) cashInfo.style.display = 'none';

            // Show relevant info
            if (this.value === 'bank' && bankInfo) {
                bankInfo.style.display = 'block';
            } else if (this.value === 'cash' && cashInfo) {
                cashInfo.style.display = 'block';
            }

            updateSubmitButton();
        });
    });
}

    function initEventHandlers() {
    // Handle donate event buttons
    const donateEventBtns = document.querySelectorAll('.donate-event-btn');
    donateEventBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            const eventId = this.dataset.eventId;
            const eventTitle = this.dataset.eventTitle;

            document.getElementById('event_id').value = eventId;

            // Scroll to donation form
            scrollToDonateForm();

            // Highlight the form briefly
            const formCard = document.querySelector('.modern-donation-card');
            if (formCard) {
                formCard.style.borderColor = '#6fbb6b';
                formCard.style.boxShadow = '0 0 30px rgba(111, 187, 107, 0.3)';
                setTimeout(() => {
                    formCard.style.borderColor = '';
                    formCard.style.boxShadow = '';
                }, 3000);
            }

            showToast(`✨ Đã chọn sự kiện: ${eventTitle}`, 'success');
        });
    });

    // Form validation and submission
        const donationForm = document.getElementById('donationForm');

        if (donationForm) {
            donationForm.addEventListener('submit', function (e) {
                const donationType = document.querySelector('input[name="donation_type"]:checked')?.value;
                const submitBtn = document.querySelector('.btn-donate');

                // 👉 Nếu quyên góp bằng điểm
                if (donationType === 'points') {
                    const pointInput = document.querySelector('input[name="point_amount"]');
                    const pointValue = parseInt(pointInput?.value || 0);

                    if (isNaN(pointValue) || pointValue < 10000) {
                        e.preventDefault();
                        showToast('⚠️ Số điểm tối thiểu để quyên góp là 10.000.', 'warning');
                        return;
                    }

                    // Cho submit nếu hợp lệ
                    return;
                }

                // 👉 Nếu quyên góp bằng tiền
                const selectedAmount = document.querySelector('input[name="amount"]:checked');
                const customAmount = parseInt(document.getElementById('custom_amount')?.value || 0);
                const selectedPayment = document.querySelector('input[name="payment_method"]:checked');

                if (!selectedAmount && (!customAmount || customAmount < 10000)) {
                    e.preventDefault();
                    showToast('⚠️ Vui lòng chọn hoặc nhập số tiền quyên góp (tối thiểu 10.000đ).', 'warning');
                    return;
                }

                if (!selectedPayment) {
                    e.preventDefault();
                    showToast('⚠️ Vui lòng chọn phương thức thanh toán.', 'warning');
                    return;
                }

                // Loading state
                if (submitBtn) {
                    submitBtn.classList.add('loading');
                    submitBtn.disabled = true;
                }

                if (selectedPayment.value === 'bank' || selectedPayment.value === 'cash') {
                    e.preventDefault();
                    handleOfflinePayment(selectedPayment.value);
                    setTimeout(() => {
                        submitBtn?.classList.remove('loading');
                        submitBtn.disabled = false;
                    }, 1000);
                } else if (selectedPayment.value === 'vnpay') {
                    e.preventDefault();
                    handleOnlinePayment(selectedPayment.value);
                }

                // MoMo → submit bình thường
            });
        }


}

    // Update submit button text based on payment method
    function updateSubmitButton() {
    const selectedPayment = document.querySelector('input[name="payment_method"]:checked');
    const btnText = document.querySelector('.btn-text');

    if (selectedPayment && btnText) {
        switch (selectedPayment.value) {
            case 'momo':
    btnText.textContent = 'Thanh toán MoMo';
    break;
    case 'vnpay':
    btnText.textContent = 'Thanh toán VNPay';
    break;
    case 'bank':
    btnText.textContent = 'Xác nhận chuyển khoản';
    break;
    case 'cash':
    btnText.textContent = 'Xác nhận quyên góp';
    break;
    default:
    btnText.textContent = 'Quyên Góp Ngay';
        }
    }
}

    // Update amount display in submit button
    function updateAmountDisplay() {
    const selectedAmount = document.querySelector('input[name="amount"]:checked');
    const customAmount = document.getElementById('custom_amount').value;
    let amount = 0;

    if (selectedAmount) {
        amount = parseInt(selectedAmount.value);
    } else if (customAmount) {
        amount = parseInt(customAmount);
    }
    
    if (amount > 0) {
        const formattedAmount = new Intl.NumberFormat('vi-VN').format(amount) + 'đ';
    const btnText = document.querySelector('.btn-text');
    if (btnText) {
            const originalText = btnText.textContent.split(' - ')[0];
    btnText.textContent = originalText + ' - ' + formattedAmount;
        }
    }
}

    // Handle offline payment methods
    function handleOfflinePayment(method) {
    const formData = new FormData(document.getElementById('donationForm'));

    fetch('process_donate.php', {
        method: 'POST',
    body: formData
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            if (method === 'bank') {
        showPaymentInstructions('bank', data);
            } else if (method === 'cash') {
        showPaymentInstructions('cash', data);
            }
        } else {
        showToast(`❌ ${data.message || 'Có lỗi xảy ra'}`, 'error');
        }
    })
    .catch(error => {
        console.error('Error:', error);
    showToast('❌ Có lỗi xảy ra trong quá trình xử lý', 'error');
    });
}

    // Handle online payment methods (MoMo, VNPay) with demo mode support
    function handleOnlinePayment(method) {
    const formData = new FormData(document.getElementById('donationForm'));

    fetch('process_donate.php', {
        method: 'POST',
    body: formData
    })
    .then(response => response.json())
    .then(data => {
        console.log('🔍 AJAX Response received:', data); // Debug line
    if (data.success) {
            if (data.payment_method === 'momo_demo') {
        // Demo mode - show success message and redirect
        showToast('✅ ' + data.message, 'success');
                setTimeout(() => {
                    if (data.redirect) {
        console.log('MoMo demo redirecting to:', data.redirect); // Debug line
    window.location.replace(data.redirect);
                    }
                }, 2000);
            } else if (data.redirect) {
        // Real payment - redirect to payment gateway
        console.log('Response data:', data); // Debug line
    console.log('Redirecting to:', data.redirect); // Debug line
    // Try different redirect method
    window.location.replace(data.redirect);
            } else {
        showToast('✅ ' + data.message, 'success');
            }
        } else {
        showToast(`❌ ${data.message || 'Có lỗi xảy ra'}`, 'error');
        }

    // Reset button
    const submitBtn = document.querySelector('.btn-donate');
    if (submitBtn) {
        submitBtn.classList.remove('loading');
    submitBtn.disabled = false;
        }
    })
    .catch(error => {
        console.error('Error:', error);
    showToast('❌ Có lỗi xảy ra trong quá trình xử lý', 'error');

    // Reset button
    const submitBtn = document.querySelector('.btn-donate');
    if (submitBtn) {
        submitBtn.classList.remove('loading');
    submitBtn.disabled = false;
        }
    });
}

    // Show modern payment instructions modal
    function showPaymentInstructions(method, data) {
    const modal = document.createElement('div');
    modal.style.cssText = `
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.8);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 10000;
    opacity: 0;
    transition: opacity 0.3s ease;
    `;

    modal.innerHTML = `
    <div style="
            background: #1e293b;
            border-radius: 20px;
            padding: 2rem;
            max-width: 600px;
            width: 90%;
            max-height: 80vh;
            overflow-y: auto;
            border: 1px solid rgba(111, 187, 107, 0.3);
            transform: scale(0.8);
            transition: transform 0.3s ease;
        ">
        <div style="text-align: center; margin-bottom: 2rem;">
            <div style="
                    width: 80px;
                    height: 80px;
                    background: linear-gradient(135deg, #6fbb6b, #28a745);
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    margin: 0 auto 1rem;
                    font-size: 2rem;
                    color: white;
                ">
                <i class="fas fa-check"></i>
            </div>
            <h3 style="color: white; margin-bottom: 0.5rem;">Đăng ký quyên góp thành công!</h3>
            <p style="color: #94a3b8;">Vui lòng làm theo hướng dẫn bên dưới</p>
        </div>

        ${method === 'bank' ? getBankInstructions(data) : getCashInstructions(data)}

        <div style="text-align: center; margin-top: 2rem;">
            <button onclick="this.closest('[style*=\"position: fixed\"]').click()" style="
            background: linear-gradient(135deg, #6fbb6b, #28a745);
            color: white;
            border: none;
            padding: 1rem 2rem;
            border-radius: 25px;
            cursor: pointer;
            font-weight: 600;
            font-size: 1rem;
                ">
            Đã hiểu
        </button>
    </div>
</div>
`;
    
    document.body.appendChild(modal);
    
    setTimeout(() => {
        modal.style.opacity = '1';
        modal.querySelector('div').style.transform = 'scale(1)';
    }, 10);
    
    modal.addEventListener('click', (e) => {
        if (e.target === modal) {
            modal.style.opacity = '0';
            modal.querySelector('div').style.transform = 'scale(0.8)';
            setTimeout(() => {
                document.body.removeChild(modal);
            }, 300);
        }
    });
}

// Get bank transfer instructions
function getBankInstructions(data) {
    return `
    < div style = "
background: rgba(111, 187, 107, 0.1);
border: 1px solid rgba(111, 187, 107, 0.3);
border - radius: 12px;
padding: 1.5rem;
margin - bottom: 1.5rem;
">
    < h6 style = "color: #6fbb6b; margin-bottom: 1rem;" >
        <i class="fas fa-university" style="margin-right: 0.5rem;"></i>
                Thông tin chuyển khoản
            </h6 >
    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; color: white;">
        <div>
            <p><strong>Ngân hàng:</strong> Vietcombank</p>
            <p><strong>Số tài khoản:</strong> 0123456789</p>
            <p><strong>Chủ tài khoản:</strong> TRUONGSACH CHARITY</p>
        </div>
        <div>
            <p><strong>Số tiền:</strong> ${new Intl.NumberFormat('vi-VN').format(data.amount)}đ</p>
            <p><strong>Nội dung CK:</strong> DONATE ${data.donation_id}</p>
            <p><strong>Mã quyên góp:</strong> #${data.donation_id}</p>
        </div>
    </div>
        </div >
    <div style="
            background: rgba(255, 193, 7, 0.1);
            border: 1px solid rgba(255, 193, 7, 0.3);
            border-radius: 12px;
            padding: 1.5rem;
            color: #ffc107;
        ">
        <h6 style="margin-bottom: 1rem;">
            <i class="fas fa-exclamation-triangle" style="margin-right: 0.5rem;"></i>
            Lưu ý quan trọng
        </h6>
        <ul style="margin: 0; padding-left: 1.5rem;">
            <li>Vui lòng chuyển khoản đúng số tiền và nội dung</li>
            <li>Sau khi chuyển khoản, gửi ảnh chụp màn hình về email: <strong>donate@convoi.org</strong></li>
            <li>Chúng tôi sẽ xác nhận trong vòng 24h</li>
        </ul>
    </div>
`;
}

// Get cash donation instructions
function getCashInstructions(data) {
    return `
    < div style = "
background: rgba(111, 187, 107, 0.1);
border: 1px solid rgba(111, 187, 107, 0.3);
border - radius: 12px;
padding: 1.5rem;
margin - bottom: 1.5rem;
color: white;
">
    < h6 style = "color: #6fbb6b; margin-bottom: 1rem;" >
        <i class="fas fa-map-marker-alt" style="margin-right: 0.5rem;"></i>
                Thông tin quyên góp trực tiếp
            </h6 >
            <p><strong>Địa chỉ:</strong> 123 Đường ABC, Quận 1, TP.HCM</p>
            <p><strong>Thời gian:</strong> Thứ 2 - Thứ 6: 8:00 - 17:00</p>
            <p><strong>Liên hệ:</strong> (028) 1234 5678</p>
            <p><strong>Mã quyên góp:</strong> #${data.donation_id}</p>
        </div >
    <div style="
            background: rgba(255, 193, 7, 0.1);
            border: 1px solid rgba(255, 193, 7, 0.3);
            border-radius: 12px;
            padding: 1.5rem;
            color: #ffc107;
        ">
        <h6 style="margin-bottom: 1rem;">
            <i class="fas fa-exclamation-triangle" style="margin-right: 0.5rem;"></i>
            Lưu ý
        </h6>
        <ul style="margin: 0; padding-left: 1.5rem;">
            <li>Vui lòng mang theo mã quyên góp khi đến</li>
            <li>Số tiền quyên góp: ${new Intl.NumberFormat('vi-VN').format(data.amount)}đ</li>
            <li>Chúng tôi sẽ cấp biên lai xác nhận</li>
        </ul>
    </div>
`;
}

// Enhanced Toast Function
function showToast(message, type = 'info') {
    const toast = document.createElement('div');
    toast.className = `TruongSach - toast TruongSach - toast - ${ type } `;
    toast.innerHTML = `
    < div class="toast-icon ${type}" >
        <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'exclamation-circle' : type === 'warning' ? 'exclamation-triangle' : 'info-circle'}"></i>
        </div >
        <div class="toast-content">
            <p class="toast-message">${message}</p>
        </div>
        <button class="toast-close">
            <i class="fas fa-times"></i>
        </button>
`;

    let container = document.querySelector('.TruongSach-toast-container');
    if (!container) {
        container = document.createElement('div');
        container.className = 'TruongSach-toast-container';
        document.body.appendChild(container);
    }

    container.appendChild(toast);

    setTimeout(() => {
        toast.classList.add('removing');
        setTimeout(() => {
            if (toast.parentNode) {
                toast.parentNode.removeChild(toast);
            }
        }, 300);
    }, 5000);

    toast.querySelector('.toast-close').addEventListener('click', () => {
        toast.classList.add('removing');
        setTimeout(() => {
            if (toast.parentNode) {
                toast.parentNode.removeChild(toast);
            }
        }, 300);
    });
}

// Function to get URL parameters
function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
}

// Function to auto-select event and amount from URL parameters
function autoSelectFromUrl() {
    console.log('🔍 autoSelectFromUrl called');
    console.log('🔍 Current URL:', window.location.href);
    console.log('🔍 URL search params:', window.location.search);

    const eventId = getUrlParameter('event_id');
    const amount = getUrlParameter('amount');

    console.log('🔍 Extracted eventId:', eventId);
    console.log('🔍 Extracted amount:', amount);

    // Auto-select event if provided
    if (eventId) {
        const eventSelect = document.getElementById('event_id');
        console.log('🔍 Event select element:', eventSelect);
        console.log('🔍 All select elements:', document.querySelectorAll('select'));
        console.log('🔍 All elements with id containing event:', document.querySelectorAll('[id*="event"]'));

        if (eventSelect) {
            console.log('🔍 Current options in select:', eventSelect.options.length);
            for (let i = 0; i < eventSelect.options.length; i++) {
                console.log(`Option ${ i }: value = "${eventSelect.options[i].value}", text = "${eventSelect.options[i].text}"`);
            }
            console.log('🔍 Setting event select value to:', eventId);
            eventSelect.value = eventId;
            console.log('🔍 Event select value after setting:', eventSelect.value);
            // Trigger change event to update any dependent elements
            eventSelect.dispatchEvent(new Event('change'));
            console.log('✅ Event selected successfully');
        } else {
            console.log('❌ Event select element not found');
        }
    }

    // Auto-select amount if provided
    if (amount) {
        console.log('🔍 Processing amount:', amount);
        // First try to select from preset amounts
        const amountRadios = document.querySelectorAll('input[name="amount"]');
        console.log('🔍 Found amount radios:', amountRadios.length);
        let amountSelected = false;

        amountRadios.forEach((radio, index) => {
            console.log(`🔍 Radio ${ index }: value = "${radio.value}", amount = "${amount}", equal = ${ radio.value == amount } `);
            // Use == instead of === to allow type coercion
            if (radio.value == amount) {
                radio.checked = true;
                amountSelected = true;
                // Trigger change event
                radio.dispatchEvent(new Event('change'));
                console.log('✅ Amount radio selected:', radio.value);

                // Also trigger visual update
                const label = document.querySelector(`label[for= "${radio.id}"]`);
                if (label) {
                    // Remove selected class from all amount cards
                    document.querySelectorAll('.amount-card').forEach(card => {
                        card.classList.remove('selected');
                    });
                    // Add selected class to current card
                    label.classList.add('selected');
                    console.log('✅ Label visual updated');
                } else {
                    console.log('❌ Label not found for radio:', radio.id);
                }

                // Also trigger the amount display update
                updateAmountDisplay();
                updateSubmitButton();
            }
        });

        // If amount not found in presets, use custom amount
        if (!amountSelected) {
            console.log('🔍 Amount not found in presets, using custom amount');
            const customAmountInput = document.getElementById('custom_amount');
            console.log('🔍 Custom amount input:', customAmountInput);
            if (customAmountInput) {
                customAmountInput.value = amount;
                // Clear any selected preset amounts
                amountRadios.forEach(radio => {
                    radio.checked = false;
                });
                // Trigger input event
                customAmountInput.dispatchEvent(new Event('input'));
                console.log('✅ Custom amount set:', amount);
            } else {
                console.log('❌ Custom amount input not found');
            }
        }
    }

    // Show success message if parameters were provided
    if (eventId || amount) {
        console.log('✅ Showing success message and scrolling');
        showToast('Thông tin đã được tự động điền từ dự án bạn chọn!', 'success');

        // Scroll to form
        setTimeout(() => {
            scrollToDonateForm();
        }, 500);
    } else {
        console.log('ℹ️ No URL parameters found');
    }
}

// Initialize when page loads
document.addEventListener('DOMContentLoaded', function() {
    console.log('🚀 DOM Content Loaded');
    updateSubmitButton();

    // Wait a bit more for all elements to be ready
    setTimeout(() => {
        console.log('🔄 Running autoSelectFromUrl');
        autoSelectFromUrl();
    }, 500);
});