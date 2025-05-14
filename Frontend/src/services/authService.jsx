// This service handles all authentication-related API calls
import api from '../utils/api';

const authService = {
  // Login function that sends credentials to the API and stores the token
  login: async (username, password) => {
    try {
      const response = await api.post('/auth/login', { username, password });
      
      // Store the token in localStorage for persistence across page refreshes
      if (response.data.token) {
        localStorage.setItem('token', response.data.token);
      }
      
      return response.data;
    } catch (error) {
      // Extract error message from API response or use a default message
      throw new Error(error.response?.data?.message || 'Login failed');
    }
  },

  // Development-only login that bypasses password verification
  devLogin: async () => {
    try {
      const response = await api.post('/auth/devlogin');
      
      // Store the token in localStorage for persistence across page refreshes
      if (response.data.token) {
        localStorage.setItem('token', response.data.token);
      }
      
      return response.data;
    } catch (error) {
      throw new Error('Development login failed');
    }
  },

  // Register function creates a new user account
  register: async (userData) => {
    try {
      const response = await api.post('/auth/register', userData);
      return response.data;
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Registration failed');
    }
  },

  // Logout removes the token from localStorage
  logout: () => {
    localStorage.removeItem('token');
  },

  // Gets current user data from the token
  getCurrentUser: async () => {
    try {
      const response = await api.get('/user/me');
      return response.data;
    } catch (error) {
      throw new Error('Failed to get user data');
    }
  }
};

export default authService;