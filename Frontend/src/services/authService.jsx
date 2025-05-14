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

  // Gets current user data from the token (simplified version)
  getCurrentUser: () => {
    const token = localStorage.getItem('token');
    if (token) {
      // In a real app, you'd decode the JWT token here
      // For now, we'll return a simple object
      return { token };
    }
    return null;
  }
};

export default authService;