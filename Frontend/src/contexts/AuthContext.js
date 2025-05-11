import React, { createContext, useState, useEffect } from 'react';
import authService from '../services/authService';

export const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [authError, setAuthError] = useState(null);

  useEffect(() => {
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
  }, []);

  const login = async (username, password) => {
    try {
      setAuthError(null);
      const response = await authService.login(username, password);
      if (response && response.token) {
        const userData = await authService.getCurrentUser();
        setUser(userData);
        return true;
      }
      return false;
    } catch (error) {
      setAuthError(error.message || 'Login failed');
      return false;
    }
  };

  const logout = () => {
    authService.logout();
    setUser(null);
  };

  const register = async (userData) => {
    try {
      setAuthError(null);
      await authService.register(userData);
      return true;
    } catch (error) {
      setAuthError(error.message || 'Registration failed');
      return false;
    }
  };

  const value = {
    user,
    loading,
    login,
    logout,
    register,
    authError,
    isAuthenticated: !!user
  };

  return (
    <AuthContext.Provider value={value}>
      {!loading && children}
    </AuthContext.Provider>
  );
};