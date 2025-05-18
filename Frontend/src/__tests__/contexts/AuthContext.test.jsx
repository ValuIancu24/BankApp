// src/__tests__/contexts/AuthContext.test.jsx
import React from 'react';
import { render, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { AuthProvider, AuthContext } from '../../contexts/AuthContext';
import { vi } from 'vitest';

// Mock the service BEFORE importing it
vi.mock('../../services/authService', () => {
  return {
    default: {
      getCurrentUser: vi.fn(),
      login: vi.fn(),
      logout: vi.fn(),
      register: vi.fn(),
    }
  };
});

// Now import the service after mocking
import authService from '../../services/authService';

// Mock localStorage
const mockLocalStorage = {
  getItem: vi.fn(),
  setItem: vi.fn(),
  removeItem: vi.fn(),
};

Object.defineProperty(window, 'localStorage', {
  value: mockLocalStorage,
  writable: true,
});

describe('AuthContext', () => {
  const mockUser = {
    id: 1,
    username: 'testuser',
    firstName: 'Test',
    lastName: 'User',
    email: 'test@example.com',
  };

  beforeEach(() => {
    vi.clearAllMocks();
    mockLocalStorage.getItem.mockReturnValue(null);
  });

  test('initializes with no user and loading state', async () => {
    let contextValue;
    
    render(
      <AuthProvider>
        <AuthContext.Consumer>
          {value => {
            contextValue = value;
            return null;
          }}
        </AuthContext.Consumer>
      </AuthProvider>
    );
    
    await waitFor(() => expect(contextValue.loading).toBe(false));
    
    expect(contextValue.user).toBeNull();
    expect(contextValue.isAuthenticated).toBe(false);
  });
});