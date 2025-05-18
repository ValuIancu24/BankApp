import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { BrowserRouter } from 'react-router-dom';
import { AuthContext } from '../../../contexts/AuthContext';
import ProtectedRoute from '../../../components/Common/ProtectedRoute';
import { vi } from 'vitest';

// Properly mock react-router-dom
vi.mock('react-router-dom', async (importOriginal) => {
  const actual = await importOriginal();
  return {
    ...actual,
    Navigate: () => <div data-testid="navigate-mock" />
  };
});

describe('ProtectedRoute Component', () => {
  test('renders children when user is authenticated', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ isAuthenticated: true }}>
          <ProtectedRoute>
            <div data-testid="protected-content">Protected Content</div>
          </ProtectedRoute>
        </AuthContext.Provider>
      </BrowserRouter>
    );
    
    expect(screen.getByTestId('protected-content')).toBeInTheDocument();
    expect(screen.queryByTestId('navigate-mock')).not.toBeInTheDocument();
  });
});