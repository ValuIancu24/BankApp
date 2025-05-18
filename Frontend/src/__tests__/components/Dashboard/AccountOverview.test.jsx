import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import AccountOverview from '../../../components/Dashboard/AccountOverview';
import { vi } from 'vitest';

describe('AccountOverview Component', () => {
  const mockAccount = {
    id: 1,
    accountNumber: 'ACC123456789',
    balance: 1000,
    currency: 'RON',
    spendingLimit: 500,
    dailyWithdrawalLimit: 2000,
    status: 'Active',
    createdAt: '2023-06-01T10:30:00Z'
  };
  
  const mockOnUpdate = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  test('renders account details correctly', () => {
    render(<AccountOverview account={mockAccount} onUpdate={mockOnUpdate} />);
    
    expect(screen.getByText('Account Details')).toBeInTheDocument();
    expect(screen.getByText('ACC123456789')).toBeInTheDocument();
    expect(screen.getByText('RON 1000.00')).toBeInTheDocument();
    expect(screen.getByText('RON 500.00')).toBeInTheDocument();
    expect(screen.getByText('RON 2000.00')).toBeInTheDocument();
    expect(screen.getByText('Active')).toBeInTheDocument();
  });
});