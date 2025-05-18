// src/__tests__/components/Dashboard/TransferMoney.test.jsx
import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { vi } from 'vitest';

// Import after mocking
import TransferMoney from '../../../components/Dashboard/TransferMoney';

describe('TransferMoney Component', () => {
  const mockAccounts = [
    {
      id: 1,
      accountNumber: 'ACC123456789',
      balance: 1000,
      currency: 'RON'
    },
    {
      id: 2,
      accountNumber: 'ACC987654321',
      balance: 500,
      currency: 'EUR'
    }
  ];
  
  const mockOnClose = vi.fn();
  const mockOnSuccess = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  test('renders transfer form with account options', () => {
    render(
      <TransferMoney 
        accounts={mockAccounts} 
        onClose={mockOnClose} 
        onSuccess={mockOnSuccess} 
      />
    );
    
    expect(screen.getByText('Transfer Money')).toBeInTheDocument();
    
    // Check fields exist
    expect(screen.getByText('From Account')).toBeInTheDocument();
    expect(screen.getByText('To Account Number')).toBeInTheDocument();
    expect(screen.getByText(/Amount/)).toBeInTheDocument();
    expect(screen.getByText('Note (Optional)')).toBeInTheDocument();
    
    // Check buttons
    expect(screen.getByRole('button', { name: 'Cancel' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Transfer' })).toBeInTheDocument();
  });
});