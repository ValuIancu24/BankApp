// Axios instance with interceptors for authentication and error handling
import axios from 'axios';

// Create an axios instance with base URL and common configuration
const api = axios.create({
  baseURL: 'http://localhost:5084/api', // Updated to match the actual backend URL
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
    // For development mode, don't actually redirect on auth errors
    console.log('API Error (suppressed in dev mode):', error);
    return Promise.reject(error);
  }
);

export default api;