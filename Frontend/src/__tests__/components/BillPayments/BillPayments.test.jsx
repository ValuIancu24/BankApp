// src/__tests__/components/BillPayments/BillPayments.test.jsx
import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { BrowserRouter } from 'react-router-dom';
import BillPayments from '../../../components/BillPayments/BillPayments';
import accountService from '../../../services/accountService';
import { vi } from 'vitest';

// Mock services and router
vi.mock('../../../services/accountService', () => ({
  default: {
    getAccounts: vi.fn(),
  }
}));

vi.mock('../../../components/Common/Header', () => ({
  default: () => <div data-testid="mock-header">Header</div>
}));

vi.mock('../../../components/Common/Footer', () => ({
  default: () => <div data-testid="mock-footer">Footer</div>
}));

const mockedNavigate = vi.fn();

describe('BillPayments Component', () => {
  const mockAccounts = [
    {
      id: 1,
      accountNumber: 'ACC123456789',
      balance: 1000,
      currency: 'RON'
    }
  ];

  beforeEach(() => {
    vi.clearAllMocks();
    accountService.getAccounts.mockResolvedValue(mockAccounts);
  });

  test('renders bill payment form with all fields', async () => {
    render(
      <BrowserRouter>
        <BillPayments />
      </BrowserRouter>
    );
    
    await waitFor(() => {
      expect(screen.getByText('Pay Your Bills')).toBeInTheDocument();
      expect(screen.getByText('Bill Payment Form')).toBeInTheDocument();
    });
    
    // Check form fields
    expect(screen.getByLabelText('From Account')).toBeInTheDocument();
    expect(screen.getByLabelText('Bill Type')).toBeInTheDocument();
    expect(screen.getByLabelText('Provider')).toBeInTheDocument();
    expect(screen.getByLabelText('Bill Number / Client ID')).toBeInTheDocument();
    expect(screen.getByLabelText(/Amount/)).toBeInTheDocument();
    
    // Check buttons
    expect(screen.getByRole('button', { name: 'Cancel' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Pay Bill' })).toBeInTheDocument();
  });
});