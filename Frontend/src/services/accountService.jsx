// This service handles all account-related operations
import api from '../utils/api';

const accountService = {
  // Get all accounts for the current user
  getAccounts: async () => {
    try {
      const response = await api.get('/account');
      return response.data;
    } catch (error) {
      throw new Error('Failed to fetch accounts');
    }
  },

  // Create a new account with specified currency
  createAccount: async (currency, initialBalance = 0) => {
    try {
      const response = await api.post('/account', { 
        currency, 
        initialBalance 
      });
      return response.data;
    } catch (error) {
      throw new Error('Failed to create account');
    }
  },

  // Get details of a specific account
  getAccountById: async (accountId) => {
    try {
      const response = await api.get(`/account/${accountId}`);
      return response.data;
    } catch (error) {
      throw new Error('Failed to fetch account details');
    }
  },

  // Update spending limit for an account
  updateSpendingLimit: async (accountId, newLimit) => {
    try {
      const response = await api.put(`/account/${accountId}/spending-limit`, { newLimit });
      return response.data;
    } catch (error) {
      throw new Error('Failed to update spending limit');
    }
  }
};

export default accountService;