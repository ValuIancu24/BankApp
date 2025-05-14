// This service handles all transaction-related operations
import api from '../utils/api';

const transactionService = {
  // Get transaction history for a specific account
  getAccountTransactions: async (accountId) => {
    try {
      const response = await api.get(`/transaction/account/${accountId}`);
      return response.data;
    } catch (error) {
      throw new Error('Failed to fetch transactions');
    }
  },

  // Create a new transaction (transfer money)
  createTransaction: async (transactionData) => {
    try {
      const response = await api.post('/transaction', transactionData);
      return response.data;
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Transaction failed');
    }
  },

  // Cancel a pending transaction
  cancelTransaction: async (transactionId) => {
    try {
      const response = await api.post(`/transaction/${transactionId}/cancel`);
      return response.data;
    } catch (error) {
      throw new Error('Failed to cancel transaction');
    }
  },

  // Mark a transaction as important
  markTransactionAsImportant: async (transactionId) => {
    try {
      const response = await api.put(`/transaction/${transactionId}/important`);
      return response.data;
    } catch (error) {
      throw new Error('Failed to mark transaction as important');
    }
  }
};

export default transactionService;