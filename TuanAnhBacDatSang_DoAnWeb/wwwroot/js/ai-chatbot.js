class TruongSachAIChatbot {
    constructor(options = {}) {
        this.options = {
            apiUrl: '/api/ai_chat.php?v=' + Date.now(),
            suggestions: [
                {
                    icon: 'fas fa-exclamation-triangle',
                    text: 'Dự án nào cần hỗ trợ gấp nhất?',
                    category: 'urgent'
                },
                {
                    icon: 'fas fa-hand-holding-heart',
                    text: 'Tôi nên quyên góp bao nhiều?',
                    category: 'donation'
                },
                {
                    icon: 'fas fa-chart-pie',
                    text: 'Tiền quyên góp được sử dụng như thế nào?',
                    category: 'transparency'
                },
                {
                    icon: 'fas fa-hands-helping',
                    text: 'Làm thế nào để tham gia tình nguyện?',
                    category: 'volunteer'
                },
                {
                    icon: 'fas fa-shield-alt',
                    text: 'Dự án này có uy tín không?',
                    category: 'credibility'
                },
                {
                    icon: 'fas fa-calendar-check',
                    text: 'Khi nào dự án hoàn thành?',
                    category: 'timeline'
                }
            ],
            ...options
        };

        this.isOpen = false;
        this.isTyping = false;
        this.sessionId = this.generateSessionId();

        this.init();
    }

    init() {
        // Prevent duplicate initialization
        if (window.D8AIInitialized) {
            return;
        }
        window.D8AIInitialized = true;

        this.createWidget();
        this.attachEventListeners();
        this.createPageSuggestions();

        // Auto show welcome message after 3 seconds
        setTimeout(() => {
            if (!this.isOpen) {
                this.showWelcomeNotification();
            }
        }, 3000);
    }

    createWidget() {
        // Remove existing widget if any
        const existingWidget = document.getElementById('TruongSach-ai-widget');
        if (existingWidget) existingWidget.remove();

        const widget = document.createElement('div');
        widget.id = 'TruongSach-ai-widget';
        widget.innerHTML = `
            <!-- Floating Button -->
            <div id="ai-chat-toggle" class="ai-chat-toggle">
                <img src="/images/chatbox.gif" alt="AI bot" class="ai-icon-gif">
                <div class="chat-pulse"></div>
                <div class="notification-badge" id="notification-badge">1</div>
            </div>
            
            <!-- Chat Modal -->
            <div id="ai-chat-modal" class="ai-chat-modal">
                <div class="ai-chat-container">
                    <!-- Header -->
                    <div class="ai-chat-header">
                        <div class="ai-avatar">
                            <i class="fas fa-school"></i>
                        </div>
                        <div class="ai-info">
                            <h4>D8AI Assistant</h4>
                            <p class="ai-status">Trí tuệ nhân tạo từ thiện thông minh</p>
                        </div>
                        <button id="ai-chat-close" class="ai-chat-close">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                    
                    <!-- Messages Area -->
                    <div id="ai-messages" class="ai-messages">
                        <div class="ai-message ai-message-bot">
                            <div class="message-avatar">
                                <i class="fas fa-school"></i>
                            </div>
                            <div class="message-content">
                                <div class="message-text">
                                    Xin chào! Tôi là D8AI - Trợ lý AI từ thiện thông minh của TRUONGSACH 🌱<br>
                                    Tôi phân tích dữ liệu real-time và hiểu sâu về các dự án để đưa ra lời khuyên tốt nhất cho bạn.<br>
                                    Hãy chọn câu hỏi gợi ý hoặc đặt câu hỏi để bắt đầu!
                                </div>
                                <div class="message-time">${this.getCurrentTime()}</div>
                            </div>
                        </div>
                    </div>
                    
                    <!-- Suggestions in Chat -->
                    <div id="ai-chat-suggestions" class="ai-chat-suggestions">
                        <div class="suggestions-title">💡 Câu hỏi phổ biến:</div>
                        <div class="suggestions-grid" id="chat-suggestions-container">
                            ${this.renderSuggestions(this.options.suggestions.slice(0, 4))}
                        </div>
                    </div>
                    
                    <!-- Input Area -->
                    <div class="ai-chat-input">
                        <div class="input-container">
                            <input 
                                type="text" 
                                id="ai-message-input" 
                                placeholder="Nhập câu hỏi của bạn..."
                                maxlength="500"
                            >
                            <button id="ai-send-btn" class="ai-send-btn" disabled>
                                <i class="fas fa-paper-plane"></i>
                            </button>
                        </div>
                        <div class="typing-indicator" id="typing-indicator">
                            <div class="typing-dots">
                                <span></span>
                                <span></span>
                                <span></span>
                            </div>
                            <span class="typing-text">AI đang soạn phản hồi...</span>
                        </div>
                        <div class="input-footer">
                            <span class="powered-by"></span>
                        </div>
                    </div>
                </div>
            </div>
        `;

        document.body.appendChild(widget);
        this.loadStyles();
    }

    createPageSuggestions() {
        // Smart suggestions based on page data
        const blogPosts = document.querySelectorAll('.post-card, .blog-post, .project-card, .charity-event');

        blogPosts.forEach((post, index) => {
            if (!post.querySelector('.ai-suggestions-inline')) {
                const postData = this.extractPostData(post);
                const smartSuggestions = this.generateSmartSuggestions(postData);

                const suggestionsContainer = document.createElement('div');
                suggestionsContainer.className = 'ai-suggestions-inline meta-ai-style';
                suggestionsContainer.innerHTML = `
                    <div class="suggestions-header">
                        <div class="ai-icon-wrapper">
                            <i class="fas fa-search"></i>
                        </div>
                        <span>Hỏi D8AI về dự án này</span>
                    </div>
                    <div class="suggestions-buttons">
                        ${this.renderInlineSuggestions(smartSuggestions)}
                    </div>
                `;

                // Insert after post content
                post.appendChild(suggestionsContainer);
            }
        });
    }

    extractPostData(postElement) {
        const data = {
            title: '',
            progress: 0,
            targetAmount: 0,
            currentAmount: 0,
            supporters: 0,
            priority: 'normal',
            category: '',
            timeRemaining: ''
        };

        // Extract title
        const titleEl = postElement.querySelector('h3, .blog-card-title, .post-title');
        if (titleEl) data.title = titleEl.textContent.trim();

        // Extract progress percentage
        const progressEl = postElement.querySelector('.progress-text, .progress-percentage');
        if (progressEl) {
            const match = progressEl.textContent.match(/(\d+(?:\.\d+)?)%/);
            if (match) data.progress = parseFloat(match[1]);
        }

        // Extract amounts
        const amountEls = postElement.querySelectorAll('.amount, .stat-number');
        amountEls.forEach(el => {
            const text = el.textContent;
            if (text.includes('đ') || text.includes('VND')) {
                const amount = parseInt(text.replace(/[^\d]/g, ''));
                if (text.includes('MỤC TIÊU') || el.parentElement.textContent.includes('mục tiêu')) {
                    data.targetAmount = amount;
                } else if (text.includes('ĐÃ UNG HỘ') || el.parentElement.textContent.includes('đã ủng hộ')) {
                    data.currentAmount = amount;
                }
            }
        });

        // Extract supporters
        const supporterEl = postElement.querySelector('.supporters, .people-count');
        if (supporterEl) {
            const match = supporterEl.textContent.match(/(\d+)\s*người/);
            if (match) data.supporters = parseInt(match[1]);
        }

        // Determine priority based on progress and urgency
        if (data.progress < 20) data.priority = 'high';
        else if (data.progress < 50) data.priority = 'medium';
        else data.priority = 'normal';

        return data;
    }

    generateSmartSuggestions(postData) {
        const suggestions = [];
        const { title, progress, priority, currentAmount, targetAmount } = postData;

        // Priority-based suggestions
        if (priority === 'high') {
            suggestions.push(`Tại sao dự án ${title} cần hỗ trợ gấp?`);
            suggestions.push(`tôi có thể giúp dự án ${title} đạt mục tiêu?`);
        } else if (priority === 'medium') {
            suggestions.push(`Dự án ${title} đã đạt được những gì?`);
            suggestions.push(`Còn thiếu bao nhiều để hoàn thành ${title}?`);
        } else {
            suggestions.push(`Tác động của dự án ${title} như thế nào?`);
            suggestions.push(`Dự án ${title} có kế hoạch mở rộng?`);
        }

        // Progress-based suggestions
        if (progress > 80) {
            suggestions.push(`Khi nào dự án ${title} hoàn thành?`);
        } else if (progress < 30) {
            suggestions.push(`tôi có thể góp sức cho dự án ${title} không?`);
        }

        // Always include credibility check
        suggestions.push(`Dự án ${title} có uy tín không?`);

        return suggestions.slice(0, 3); // Limit to 3 suggestions
    }

    renderSuggestions(suggestions) {
        return suggestions.map(suggestion => `
            <button class="suggestion-btn" data-suggestion="${suggestion.text}">
                <i class="${suggestion.icon}"></i>
                <span>${suggestion.text}</span>
            </button>
        `).join('');
    }

    renderInlineSuggestions(suggestions) {
        return suggestions.map(text => `
            <button class="inline-suggestion-btn" data-suggestion="${text}">
                ${text}
            </button>
        `).join('');
    }

    attachEventListeners() {
        // Toggle chat
        document.getElementById('ai-chat-toggle').addEventListener('click', () => {
            this.toggleChat();
        });

        // Close chat
        document.getElementById('ai-chat-close').addEventListener('click', () => {
            this.closeChat();
        });

        // Close on backdrop click
        document.getElementById('ai-chat-modal').addEventListener('click', (e) => {
            if (e.target.id === 'ai-chat-modal') {
                this.closeChat();
            }
        });

        // Send message
        document.getElementById('ai-send-btn').addEventListener('click', () => {
            this.sendMessage();
        });

        // Enter key to send
        document.getElementById('ai-message-input').addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });

        // Input validation
        document.getElementById('ai-message-input').addEventListener('input', (e) => {
            const sendBtn = document.getElementById('ai-send-btn');
            sendBtn.disabled = !e.target.value.trim();
        });

        // Suggestion clicks in chat
        document.addEventListener('click', (e) => {
            if (e.target.closest('.suggestion-btn, .inline-suggestion-btn')) {
                const suggestionText = e.target.closest('button').dataset.suggestion;
                this.handleSuggestionClick(suggestionText);
            }
        });

        // ESC key to close
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isOpen) {
                this.closeChat();
            }
        });
    }

    toggleChat() {
        if (this.isOpen) {
            this.closeChat();
        } else {
            this.openChat();
        }
    }

    openChat() {
        const modal = document.getElementById('ai-chat-modal');
        modal.classList.add('active');
        this.isOpen = true;

        // Hide notification badge
        const badge = document.getElementById('notification-badge');
        badge.style.display = 'none';

        // Focus input
        setTimeout(() => {
            document.getElementById('ai-message-input').focus();
        }, 300);

        // Analytics event
        this.trackEvent('chat_opened');
    }

    closeChat() {
        const modal = document.getElementById('ai-chat-modal');
        modal.classList.remove('active');
        this.isOpen = false;

        this.trackEvent('chat_closed');
    }

    handleSuggestionClick(suggestionText) {
        // Open chat if not open
        if (!this.isOpen) {
            this.openChat();
        }

        // Set input value and send
        setTimeout(() => {
            const input = document.getElementById('ai-message-input');
            input.value = suggestionText;
            this.sendMessage();
        }, this.isOpen ? 100 : 400);

        this.trackEvent('suggestion_clicked', { suggestion: suggestionText });
    }

    async sendMessage() {
        const input = document.getElementById('ai-message-input');
        const message = input.value.trim();

        if (!message || this.isTyping) return;

        // Add user message
        this.addMessage(message, 'user');
        input.value = '';
        document.getElementById('ai-send-btn').disabled = true;

        // Show typing indicator
        this.showTyping();

        try {
            const context = this.getPageContext();
            const response = await this.callAPI(message, context);

            if (response.success) {
                this.addMessage(response.response, 'bot');
                this.trackEvent('message_success');
            } else {
                this.addMessage(
                    `Xin lỗi, tôi gặp sự cố: ${response.error}. Vui lòng thử lại sau.`,
                    'bot',
                    'error'
                );
                this.trackEvent('message_error', { error: response.error });
            }
        } catch (error) {
            this.addMessage(
                'Không thể kết nối với AI. Vui lòng kiểm tra kết nối mạng và thử lại.',
                'bot',
                'error'
            );
            this.trackEvent('network_error', { error: error.message });
        } finally {
            this.hideTyping();
        }
    }

    async callAPI(message, context) {
        try {
            // Process and enhance the context with smart data
            const enhancedContext = this.processSmartContext(context, message);

            const response = await fetch(this.options.apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    message: message,
                    context: enhancedContext,
                    session_id: this.sessionId,
                    timestamp: Date.now()
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }

            const result = await response.json();
            console.log('API Response:', result);
            return result;

        } catch (error) {
            console.error('API Call Error:', error);
            throw error;
        }
    }

    processSmartContext(context, message) {
        const pageData = this.analyzePageData();
        const priorityProjects = this.calculatePriorities(pageData);
        const insights = this.generateInsights(pageData);

        return JSON.stringify({
            ...JSON.parse(context || '{}'),
            smart_analysis: {
                priority_projects: priorityProjects,
                urgent_needs: this.getUrgentNeeds(pageData),
                success_stories: this.getSuccessStories(pageData),
                donation_insights: insights.donations,
                user_intent: this.detectUserIntent(message)
            },
            real_time_data: {
                total_projects: pageData.length,
                completion_rate: this.calculateCompletionRate(pageData),
                most_supported: this.getMostSupported(pageData),
                timestamp: Date.now()
            }
        });
    }

    analyzePageData() {
        const projects = [];
        const projectCards = document.querySelectorAll('.post-card-modern, .project-card, .charity-event');

        projectCards.forEach(card => {
            const data = this.extractPostData(card);
            if (data.title) projects.push(data);
        });

        return projects;
    }

    calculatePriorities(projects) {
        return projects
            .filter(p => p.priority === 'high')
            .sort((a, b) => a.progress - b.progress)
            .slice(0, 3)
            .map(p => ({
                title: p.title,
                progress: p.progress,
                urgency_reason: p.progress < 10 ? 'Vừa bắt đầu, cần hỗ trợ khởi động' :
                    p.progress < 30 ? 'Tiến độ chậm, cần đẩy mạnh' :
                        'Gần hoàn thành, cần hỗ trợ cuối'
            }));
    }

    getUrgentNeeds(projects) {
        return projects
            .filter(p => p.progress > 0 && p.progress < 30)
            .map(p => ({
                title: p.title,
                needed: p.targetAmount - p.currentAmount,
                progress: p.progress
            }));
    }

    detectUserIntent(message) {
        const intents = {
            donation: /quyên góp|ủng hộ|cho tiền|donate/i,
            volunteer: /tình nguyện|tham gia|volunteer|giúp đỡ/i,
            info: /thông tin|chi tiết|details|tìm hiểu/i,
            credibility: /uy tín|tin cậy|đáng tin|chất lượng/i,
            impact: /tác động|kết quả|hiệu quả|thành công/i
        };

        for (const [intent, pattern] of Object.entries(intents)) {
            if (pattern.test(message)) return intent;
        }
        return 'general';
    }

    calculateCompletionRate(projects) {
        if (projects.length === 0) return 0;
        const avgProgress = projects.reduce((sum, p) => sum + p.progress, 0) / projects.length;
        return Math.round(avgProgress);
    }

    generateInsights(projects) {
        const totalTarget = projects.reduce((sum, p) => sum + p.targetAmount, 0);
        const totalCurrent = projects.reduce((sum, p) => sum + p.currentAmount, 0);

        return {
            donations: {
                total_raised: totalCurrent,
                total_target: totalTarget,
                completion_percentage: totalTarget > 0 ? (totalCurrent / totalTarget * 100).toFixed(1) : 0,
                average_donation: projects.length > 0 ? (totalCurrent / projects.length).toFixed(0) : 0
            }
        };
    }

    getSuccessStories(projects) {
        return projects
            .filter(p => p.progress > 80)
            .map(p => ({
                title: p.title,
                progress: p.progress,
                supporters: p.supporters
            }));
    }

    getMostSupported(projects) {
        if (projects.length === 0) return null;
        return projects.reduce((max, p) => p.supporters > max.supporters ? p : max);
    }

    getPageContext() {
        // Extract page context for better AI responses
        const context = {
            page_title: document.title,
            page_url: window.location.href,
            projects: []
        };

        // Extract project information from page
        const projectCards = document.querySelectorAll('.project-card, .charity-event');
        projectCards.forEach(card => {
            const title = card.querySelector('h3, .title')?.textContent?.trim();
            const description = card.querySelector('.description, p')?.textContent?.trim();
            const progress = card.querySelector('.progress')?.textContent?.trim();
            const amount = card.querySelector('.amount')?.textContent?.trim();

            if (title) {
                context.projects.push({
                    title,
                    description: description?.substring(0, 200),
                    progress,
                    amount
                });
            }
        });

        return JSON.stringify(context);
    }

    addMessage(content, type, status = 'normal') {
        const messagesContainer = document.getElementById('ai-messages');
        const messageElement = document.createElement('div');
        messageElement.className = `ai-message ai-message-${type} ${status === 'error' ? 'message-error' : ''}`;

        if (type === 'user') {
            messageElement.innerHTML = `
                <div class="message-content">
                    <div class="message-text">${this.escapeHtml(content)}</div>
                    <div class="message-time">${this.getCurrentTime()}</div>
                </div>
                <div class="message-avatar">
                    <i class="fas fa-user"></i>
                </div>
            `;
        } else {
            messageElement.innerHTML = `
                <div class="message-avatar">
                    <i class="fas fa-school"></i>
                </div>
                <div class="message-content">
                    <div class="message-text">${this.formatBotMessage(content)}</div>
                    <div class="message-time">${this.getCurrentTime()}</div>
                    ${status !== 'error' ? '<div class="message-actions"><button class="copy-btn" onclick="navigator.clipboard.writeText(this.closest(\'.message-content\').querySelector(\'.message-text\').textContent)"><i class="fas fa-copy"></i></button></div>' : ''}
                </div>
            `;
        }

        messagesContainer.appendChild(messageElement);

        // Scroll to bottom with animation
        setTimeout(() => {
            messagesContainer.scrollTo({
                top: messagesContainer.scrollHeight,
                behavior: 'smooth'
            });
        }, 100);

        // Hide suggestions after first message
        if (type === 'user') {
            const suggestionsContainer = document.getElementById('ai-chat-suggestions');
            if (suggestionsContainer) {
                suggestionsContainer.style.display = 'none';
            }
        }
    }

    formatBotMessage(content) {
        // Format AI response with basic markdown support
        return content
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            .replace(/\*(.*?)\*/g, '<em>$1</em>')
            .replace(/`(.*?)`/g, '<code>$1</code>')
            .replace(/\n/g, '<br>')
            .replace(/(\d+\.)\s/g, '<br>$1 ') // Format numbered lists
            .replace(/- /g, '<br>• '); // Format bullet points
    }

    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    showTyping() {
        this.isTyping = true;
        const indicator = document.getElementById('typing-indicator');
        const statusText = document.querySelector('.ai-status');

        indicator.classList.add('active');
        if (statusText) statusText.textContent = 'Đang soạn phản hồi...';
    }

    hideTyping() {
        this.isTyping = false;
        const indicator = document.getElementById('typing-indicator');
        const statusText = document.querySelector('.ai-status');

        indicator.classList.remove('active');
        if (statusText) statusText.textContent = 'Trí tuệ nhân tạo từ thiện thông minh';
    }

    showWelcomeNotification() {
        const badge = document.getElementById('notification-badge');
        const toggle = document.getElementById('ai-chat-toggle');

        badge.style.display = 'block';
        toggle.classList.add('has-notification');

        // Create floating message
        const notification = document.createElement('div');
        notification.className = 'ai-welcome-notification';
        notification.innerHTML = `
            <div class="notification-content">
                <strong>🤖 AI Assistant</strong>
                <p>Xin chào! Tôi có thể giúp bạn tìm hiểu về các dự án từ thiện.</p>
                <button class="notification-close">×</button>
            </div>
        `;

        document.body.appendChild(notification);

        // Auto hide after 8 seconds
        setTimeout(() => {
            notification.remove();
        }, 8000);

        // Close notification
        notification.querySelector('.notification-close').addEventListener('click', () => {
            notification.remove();
        });
    }

    getCurrentTime() {
        return new Date().toLocaleTimeString('vi-VN', {
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    generateSessionId() {
        return 'TruongSach_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
    }

    trackEvent(eventName, data = {}) {
        // Analytics tracking
        if (typeof gtag !== 'undefined') {
            gtag('event', eventName, {
                event_category: 'AI_Chatbot',
                event_label: 'TruongSach_Assistant',
                ...data
            });
        }

        console.log('AI Chatbot Event:', eventName, data);
    }

    loadStyles() {
        if (document.getElementById('TruongSach-ai-styles')) return;

        const style = document.createElement('style');
        style.id = 'TruongSach-ai-styles';
        style.textContent = this.getStyles();
        document.head.appendChild(style);
    }

    getStyles() {
        return `
            /* TruongSach AI Chatbot Styles */
            #TruongSach-ai-widget {
                position: fixed;
                bottom: 20px;
                right: 20px;
                z-index: 999999;
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }
            .ai-icon-gif {
                width: 100px;  /* chỉnh kích thước ảnh tùy ý */
                height: auto;
                object-fit: contain;
            }

            .ai-chat-toggle {
                background: none;          /* bỏ nền */
                border: none;              /* bỏ viền */
                box-shadow: none;          /* bỏ đổ bóng */
                width: auto;
                height: auto;
                padding: 0;
                transform: none;
            }

            
            .ai-chat-toggle:hover {
                transform: scale(1.1);
            }
            
            .ai-chat-toggle.has-notification {
                animation: bounce 2s infinite;
            }
            
            .chat-pulse {
                position: absolute;
                inset: -10px;
                border-radius: 50%;
                background: radial-gradient(circle, rgba(40, 167, 69, 0.3) 0%, transparent 70%);
                animation: pulse 2s ease-in-out infinite;
            }
            
            .notification-badge {
                position: absolute;
                top: -5px;
                right: -5px;
                background: #dc3545;
                color: white;
                border-radius: 50%;
                width: 20px;
                height: 20px;
                display: none;
                align-items: center;
                justify-content: center;
                font-size: 12px;
                font-weight: bold;
                border: 2px solid white;
            }
            
            .ai-chat-modal {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.5);
                backdrop-filter: blur(5px);
                display: none;
                align-items: flex-end;
                justify-content: flex-end;
                padding: 20px;
                opacity: 0;
                transition: opacity 0.3s ease;
            }
            
            .ai-chat-modal.active {
                display: flex;
                opacity: 1;
            }
            
            .ai-chat-container {
                width: 400px;
                height: 600px;
                background: white;
                border-radius: 20px;
                box-shadow: 0 20px 60px rgba(0, 0, 0, 0.15);
                display: flex;
                flex-direction: column;
                transform: translateY(20px);
                transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                overflow: hidden;
            }
            
            .ai-chat-modal.active .ai-chat-container {
                transform: translateY(0);
            }
            
            .ai-chat-header {
                background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
                color: white;
                padding: 20px;
                display: flex;
                align-items: center;
                gap: 15px;
                position: relative;
            }
            
            .ai-avatar {
                width: 45px;
                height: 45px;
                background: rgba(255, 255, 255, 0.2);
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                font-size: 20px;
                border: 2px solid rgba(255, 255, 255, 0.3);
            }
            
            .ai-info h4 {
                margin: 0;
                font-size: 16px;
                font-weight: 600;
            }
            
            .ai-status {
                margin: 0;
                font-size: 12px;
                opacity: 0.9;
            }
            
            .ai-chat-close {
                background: none;
                border: none;
                color: white;
                font-size: 18px;
                cursor: pointer;
                margin-left: auto;
                padding: 8px;
                border-radius: 50%;
                transition: background 0.2s;
            }
            
            .ai-chat-close:hover {
                background: rgba(255, 255, 255, 0.1);
            }
            
            .ai-messages {
                flex: 1;
                padding: 20px;
                overflow-y: auto;
                display: flex;
                flex-direction: column;
                gap: 16px;
                scroll-behavior: smooth;
            }
            
            .ai-message {
                display: flex;
                gap: 12px;
                max-width: 85%;
                animation: slideIn 0.3s ease-out;
            }
            
            .ai-message-user {
                align-self: flex-end;
                flex-direction: row-reverse;
            }
            
            .ai-message-bot {
                align-self: flex-start;
            }
            
            .message-avatar {
                width: 32px;
                height: 32px;
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                font-size: 14px;
                flex-shrink: 0;
            }
            
            .ai-message-user .message-avatar {
                background: #007bff;
                color: white;
            }
            
            .ai-message-bot .message-avatar {
                background: linear-gradient(135deg, #28a745, #20c997);
                color: white;
            }
            
            .message-content {
                flex: 1;
                position: relative;
            }
            
            .message-text {
                background: #f8f9fa;
                padding: 12px 16px;
                border-radius: 18px;
                font-size: 14px;
                line-height: 1.5;
                word-wrap: break-word;
                position: relative;
            }
            
            .ai-message-user .message-text {
                background: #007bff;
                color: white;
            }
            
            .message-error .message-text {
                background: #f8d7da;
                color: #721c24;
                border: 1px solid #f5c6cb;
            }
            
            .message-time {
                font-size: 11px;
                color: #6c757d;
                margin-top: 4px;
                text-align: right;
            }
            
            .ai-message-user .message-time {
                text-align: left;
            }
            
            .message-actions {
                margin-top: 4px;
                display: flex;
                gap: 8px;
                justify-content: flex-end;
            }
            
            .copy-btn {
                background: none;
                border: none;
                color: #6c757d;
                cursor: pointer;
                padding: 4px;
                border-radius: 4px;
                font-size: 12px;
                transition: background 0.2s;
            }
            
            .copy-btn:hover {
                background: #e9ecef;
            }
            
            .ai-chat-suggestions {
                padding: 15px 20px;
                border-top: 1px solid #e9ecef;
                background: #f8f9fa;
            }
            
            .suggestions-title {
                font-size: 13px;
                font-weight: 600;
                color: #495057;
                margin-bottom: 12px;
            }
            
            .suggestions-grid {
                display: grid;
                grid-template-columns: 1fr 1fr;
                gap: 8px;
            }
            
            .suggestion-btn {
                background: white;
                border: 1px solid #dee2e6;
                border-radius: 12px;
                padding: 10px 12px;
                font-size: 12px;
                text-align: left;
                cursor: pointer;
                transition: all 0.2s;
                color: #495057;
                line-height: 1.3;
                display: flex;
                align-items: center;
                gap: 8px;
            }
            
            .suggestion-btn:hover {
                background: #28a745;
                color: white;
                border-color: #28a745;
                transform: translateY(-2px);
            }
            
            .suggestion-btn i {
                font-size: 14px;
                opacity: 0.7;
            }
            
            .ai-chat-input {
                padding: 20px;
                border-top: 1px solid #e9ecef;
                background: white;
            }
            
            .input-container {
                display: flex;
                gap: 12px;
                align-items: flex-end;
            }
            
            #ai-message-input {
                flex: 1;
                border: 2px solid #e9ecef;
                border-radius: 20px;
                padding: 12px 20px;
                font-size: 14px;
                outline: none;
                transition: border-color 0.2s;
                resize: none;
                min-height: 44px;
                max-height: 100px;
            }
            
            #ai-message-input:focus {
                border-color: #28a745;
            }
            
            .ai-send-btn {
                width: 44px;
                height: 44px;
                background: #28a745;
                color: white;
                border: none;
                border-radius: 50%;
                cursor: pointer;
                display: flex;
                align-items: center;
                justify-content: center;
                transition: all 0.2s;
                flex-shrink: 0;
            }
            
            .ai-send-btn:hover:not(:disabled) {
                background: #218838;
                transform: scale(1.05);
            }
            
            .ai-send-btn:disabled {
                opacity: 0.5;
                cursor: not-allowed;
                transform: none;
            }
            
            .typing-indicator {
                display: none;
                align-items: center;
                gap: 8px;
                margin-top: 8px;
                color: #6c757d;
                font-size: 12px;
            }
            
            .typing-indicator.active {
                display: flex;
            }
            
            .typing-dots {
                display: flex;
                gap: 4px;
            }
            
            .typing-dots span {
                width: 6px;
                height: 6px;
                background: #6c757d;
                border-radius: 50%;
                animation: typing 1.4s infinite;
            }
            
            .typing-dots span:nth-child(2) {
                animation-delay: 0.2s;
            }
            
            .typing-dots span:nth-child(3) {
                animation-delay: 0.4s;
            }
            
            .input-footer {
                margin-top: 8px;
                text-align: center;
            }
            
            .powered-by {
                font-size: 11px;
                color: #9ca3af;
            }
            
            /* Meta AI Style Inline Suggestions */
            .ai-suggestions-inline {
                margin-top: 20px;
                padding: 0;
                background: none;
                border: none;
                border-radius: 0;
            }
            
            .ai-suggestions-inline.meta-ai-style {
                background: linear-gradient(135deg, #f0f2f5 0%, #ffffff 100%);
                border: 1px solid rgba(0, 0, 0, 0.1);
                border-radius: 16px;
                padding: 16px;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                transition: all 0.3s ease;
            }
            
            .ai-suggestions-inline.meta-ai-style:hover {
                box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
            }
            
            .suggestions-header {
                display: flex;
                align-items: center;
                gap: 10px;
                margin-bottom: 16px;
                color: #1877f2;
                font-weight: 600;
                font-size: 15px;
                position: relative;
            }
            
            .ai-icon-wrapper {
                width: 32px;
                height: 32px;
                background: linear-gradient(135deg, #1877f2, #42a5f5);
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                color: white;
                font-size: 14px;
                box-shadow: 0 2px 8px rgba(24, 119, 242, 0.3);
            }
            

            
            .suggestions-buttons {
                display: flex;
                flex-direction: column;
                gap: 8px;
            }
            
            .inline-suggestion-btn {
                background: white;
                border: 1px solid rgba(0, 0, 0, 0.1);
                color: #1c1e21;
                border-radius: 20px;
                padding: 12px 16px;
                font-size: 13px;
                cursor: pointer;
                transition: all 0.2s cubic-bezier(0.2, 0, 0, 1);
                white-space: nowrap;
                text-align: left;
                position: relative;
                overflow: hidden;
                box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
                font-weight: 500;
            }
            
            .inline-suggestion-btn::before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: linear-gradient(90deg, transparent, rgba(24, 119, 242, 0.1), transparent);
                transition: left 0.5s;
            }
            
            .inline-suggestion-btn:hover::before {
                left: 100%;
            }
            
            .inline-suggestion-btn:hover {
                background: #1877f2;
                color: white;
                transform: translateY(-2px);
                box-shadow: 0 4px 12px rgba(24, 119, 242, 0.3);
                border-color: #1877f2;
            }
            
            .inline-suggestion-btn:active {
                transform: translateY(0);
                box-shadow: 0 2px 6px rgba(24, 119, 242, 0.2);
            }
            
            /* Welcome Notification */
            .ai-welcome-notification {
                position: fixed;
                bottom: 100px;
                right: 20px;
                z-index: 999998;
                animation: slideInUp 0.5s ease-out;
            }
            
            .notification-content {
                background: white;
                border-radius: 12px;
                padding: 16px;
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.15);
                border: 1px solid #e9ecef;
                max-width: 280px;
                position: relative;
            }
            
            .notification-close {
                position: absolute;
                top: 8px;
                right: 8px;
                background: none;
                border: none;
                font-size: 16px;
                cursor: pointer;
                color: #6c757d;
                width: 24px;
                height: 24px;
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
            }
            
            /* Animations */
            @keyframes pulse {
                0%, 100% { opacity: 1; }
                50% { opacity: 0.5; }
            }
            
            @keyframes bounce {
                0%, 20%, 50%, 80%, 100% { transform: translateY(0); }
                40% { transform: translateY(-10px); }
                60% { transform: translateY(-5px); }
            }
            
            @keyframes typing {
                0%, 60%, 100% { opacity: 0.3; }
                30% { opacity: 1; }
            }
            
            @keyframes slideIn {
                from { opacity: 0; transform: translateY(10px); }
                to { opacity: 1; transform: translateY(0); }
            }
            
            @keyframes slideInUp {
                from { opacity: 0; transform: translateY(20px); }
                to { opacity: 1; transform: translateY(0); }
            }
            
            /* Mobile Responsive */
            @media (max-width: 768px) {
                #TruongSach-ai-widget {
                    bottom: 15px;
                    right: 15px;
                }
                
                .ai-chat-toggle {
                    width: 55px;
                    height: 55px;
                    font-size: 22px;
                }
                
                .ai-chat-modal {
                    padding: 10px;
                    align-items: stretch;
                    justify-content: stretch;
                }
                
                .ai-chat-container {
                    width: 100%;
                    height: 100%;
                    max-height: 90vh;
                    border-radius: 15px;
                }
                
                .suggestions-grid {
                    grid-template-columns: 1fr;
                }
                
                .suggestions-buttons {
                    flex-direction: column;
                }
                
                .ai-welcome-notification {
                    right: 15px;
                    bottom: 80px;
                }
                
                .notification-content {
                    max-width: calc(100vw - 30px);
                }
            }
            
            @media (max-width: 480px) {
                .ai-chat-header {
                    padding: 15px;
                }
                
                .ai-messages {
                    padding: 15px;
                }
                
                .ai-chat-input {
                    padding: 15px;
                }
                
                .suggestions-buttons {
                    gap: 6px;
                }
                
                .inline-suggestion-btn {
                    font-size: 11px;
                    padding: 6px 12px;
                }
            }
        `;
    }
}

// Auto-initialize when DOM is ready (prevent duplicates)
document.addEventListener('DOMContentLoaded', () => {
    if (!window.TruongSachAI && !window.D8AIInitialized) {
        window.TruongSachAI = new TruongSachAIChatbot();
    }
});

// Export for manual initialization
window.TruongSachAIChatbot = TruongSachAIChatbot; 