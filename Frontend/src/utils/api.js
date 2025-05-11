// Axios instance with interceptors for authentication and error handling
import axios from 'axios';

// Create an axios instance with base URL and common configuration
const api = axios.create({
  baseURL: 'https://localhost:7252/api', // Update this to match your API URL
  headers: {
    'Content-Type': 'application/json'
  }
});

// Request interceptor to add authentication token to all requests
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle common error responses
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    // Handle 401 Unauthorized responses (token expired or invalid)
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default api;