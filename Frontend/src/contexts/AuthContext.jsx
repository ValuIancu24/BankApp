import React, { createContext, useState, useEffect } from 'react';
import authService from '../services/authService';

export const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  // DEVELOPMENT MODE: Force authentication to be true
  const [user, setUser] = useState({ 
    id: 1, 
    username: 'testuser', 
    firstName: 'Test', 
    lastName: 'User',
    email: 'test@example.com',
    isActive: true,
    phoneNumber: '0712345678',
    city: 'Craiova'
  });
  const [loading, setLoading] = useState(false);
  const [authError, setAuthError] = useState(null);

  useEffect(() => {
    // Authentication check disabled for development
    /*
    const initAuth = async () => {
      const token = localStorage.getItem('token');
      if (token) {
        try {
          const userData = await authService.getCurrentUser();
          setUser(userData);
        } catch (error) {
          // Token is invalid or expired
          console.error('Authentication error:', error);
          localStorage.removeItem('token');
        }
      }
      setLoading(false);
    };
    initAuth();
    */
    
    // Store fake user data in localStorage so Profile component can access it
    localStorage.setItem('userData', JSON.stringify({ 
      id: 1, 
      username: 'testuser', 
      firstName: 'Test', 
      lastName: 'User',
      email: 'test@example.com',
      isActive: true,
      phoneNumber: '0712345678',
      city: 'Craiova'
    }));
    
    setLoading(false);
  }, []);

  const login = async (username, password) => {
    // Always return successful login for development
    return true;
  };

  const logout = () => {
    // No real logout in development mode
    console.log('Logout clicked - disabled in development mode');
  };

  const register = async (userData) => {
    // Always return successful registration for development
    return true;
  };

  const value = {
    user,
    loading,
    login,
    logout,
    register,
    authError,
    isAuthenticated: true  // Force this to always be true for development
  };

  return (
    <AuthContext.Provider value={value}>
      {!loading && children}
    </AuthContext.Provider>
  );
};