// This service handles all account-related operations
import api from '../utils/api';

// Mock data for development
const mockAccounts = [
  {
    id: 1,
    accountNumber: 'RO49BANK1000200030004',
    balance: 5000.75,
    currency: 'RON',
    spendingLimit: 2000,
    dailyWithdrawalLimit: 1000,
    status: 'Active',
    createdAt: '2025-01-15T10:30:00'
  },
  {
    id: 2,
    accountNumber: 'RO49BANK1000200030005',
    balance: 2500.50,
    currency: 'EUR',
    spendingLimit: 1500,
    dailyWithdrawalLimit: 800,
    status: 'Active',
    createdAt: '2025-02-20T14:15:00'
  }
];

const accountService = {
  // Get all accounts for the current user
  getAccounts: async () => {
    try {
      // For development, return mock data
      return mockAccounts;
      
      // When ready to connect to backend, use:
      // const response = await api.get('/account');
      // return response.data;
    } catch (error) {
      throw new Error('Failed to fetch accounts');
    }
  },

  // Create a new account with specified currency
  createAccount: async (currency, initialBalance = 0) => {
    try {
      // For development, return a mock new account
      const newAccount = {
        id: Date.now(), // Generate unique ID
        accountNumber: `RO49BANK${Math.floor(Math.random() * 10000000000)}`,
        balance: initialBalance,
        currency,
        spendingLimit: 2000,
        dailyWithdrawalLimit: 1000,
        status: 'Active',
        createdAt: new Date().toISOString()
      };
      
      mockAccounts.push(newAccount);
      return newAccount;
      
      // When ready to connect to backend, use:
      // const response = await api.post('/account', { currency, initialBalance });
      // return response.data;
    } catch (error) {
      throw new Error('Failed to create account');
    }
  },

  // Get details of a specific account
  getAccountById: async (accountId) => {
    try {
      // For development, return mock account
      const account = mockAccounts.find(acc => acc.id === accountId);
      return account || null;
      
      // When ready to connect to backend, use:
      // const response = await api.get(`/account/${accountId}`);
      // return response.data;
    } catch (error) {
      throw new Error('Failed to fetch account details');
    }
  },

  // Update spending limit for an account
  updateSpendingLimit: async (accountId, newLimit) => {
    try {
      // For development, update mock account
      const account = mockAccounts.find(acc => acc.id === accountId);
      if (account) {
        account.spendingLimit = newLimit;
      }
      return { message: 'Spending limit updated successfully' };
      
      // When ready to connect to backend, use:
      // const response = await api.put(`/account/${accountId}/spending-limit`, { newLimit });
      // return response.data;
    } catch (error) {
      throw new Error('Failed to update spending limit');
    }
  }
};

export default accountService;