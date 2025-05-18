// src/__tests__/components/Profile/ChangePassword.test.jsx
import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import ChangePassword from '../../../components/Profile/ChangePassword';
import { vi } from 'vitest';

describe('ChangePassword Component', () => {
  const mockOnClose = vi.fn();
  
  beforeEach(() => {
    vi.clearAllMocks();
  });

  test('renders change password form with all fields', () => {
    render(<ChangePassword onClose={mockOnClose} />);
    
    // Check for heading and buttons
    expect(screen.getByRole('heading', { name: 'Change Password' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Cancel' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /change password/i })).toBeInTheDocument();
    
    // Check for labels
    expect(screen.getByText('Current Password')).toBeInTheDocument();
    expect(screen.getByText('New Password')).toBeInTheDocument();
    expect(screen.getByText('Confirm New Password')).toBeInTheDocument();
  });

  test('closes the form when cancel is clicked', () => {
    render(<ChangePassword onClose={mockOnClose} />);
    
    fireEvent.click(screen.getByRole('button', { name: 'Cancel' }));
    
    expect(mockOnClose).toHaveBeenCalled();
  });
});