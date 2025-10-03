// react-app/src/services/api-service.js
class ApiService {
    constructor() {
        this.config = null;
        this.initialized = false;
        this.initPromise = null;
    }

    async initialize() {
        if (this.initialized) return this.config;
        if (this.initPromise) return this.initPromise;

        this.initPromise = window.electronAPI.getBackendConfig()
            .then(config => {
                this.config = config;
                this.initialized = true;
                return config;
            })
            .catch(error => {
                console.error('Failed to get backend config:', error);
                throw error;
            });

        return this.initPromise;
    }

    async callAPI(endpoint, method = 'POST', body = null) {
        await this.initialize();

        const url = `${this.config.baseUrl}/api/system/${endpoint}`;
        const options = {
            method,
            headers: {
                'Content-Type': 'application/json',
                'X-API-Key': this.config.apiKey
            }
        };

        if (body && method !== 'GET') {
            options.body = JSON.stringify(body);
        }

        try {
            const response = await fetch(url, options);
            
            if (!response.ok) {
                throw new Error(`API call failed: ${response.status} ${response.statusText}`);
            }

            return await response.json();
        } catch (error) {
            console.error(`API call to ${endpoint} failed:`, error);
            throw error;
        }
    }

    // Convenience methods for each endpoint
    async test() {
        return this.callAPI('test', 'GET');
    }

    async getSystemInfo() {
        return this.callAPI('sysinfo', 'GET');
    }

    async createRestorePoint() {
        return this.callAPI('restore-point');
    }

    async optimizeRegistry() {
        return this.callAPI('optimize-registry');
    }

    async optimizeTaskScheduler() {
        return this.callAPI('optimize-taskscheduler');
    }

    async disableUpdates() {
        return this.callAPI('disable-updates');
    }

    async lowerVisuals() {
        return this.callAPI('lower-visuals');
    }

    async setDarkMode() {
        return this.callAPI('set-darkmode');
    }

    async setPowerPlan() {
        return this.callAPI('set-powerplan');
    }

    async optimizeServices(options) {
        return this.callAPI('optimize-services', 'POST', options);
    }

    async debloatApps(options) {
        return this.callAPI('debloat-apps', 'POST', options);
    }

    async cleanup(options) {
        return this.callAPI('cleanup', 'POST', options);
    }
}

export default new ApiService();