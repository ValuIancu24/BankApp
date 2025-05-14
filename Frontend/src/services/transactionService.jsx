// This service handles all transaction-related operations
import api from '../utils/api';

// Mock data for development
const mockTransactions = [
  {
    id: 1,
    amount: 500.00,
    currency: 'RON',
    type: 'Deposit',
    note: 'Salary payment',
    status: 'Completed',
    isImportant: true,
    createdAt: '2025-05-10T09:30:00',
    fromAccountId: 1,
    fromAccountNumber: 'RO49BANK1000200030004'
  },
  {
    id: 2,
    amount: 150.25,
    currency: 'RON',
    type: 'Withdrawal',
    note: 'ATM withdrawal',
    status: 'Completed',
    isImportant: false,
    createdAt: '2025-05-12T14:20:00',
    fromAccountId: 1,
    fromAccountNumber: 'RO49BANK1000200030004'
  },
  {
    id: 3,
    amount: 300.00,
    currency: 'RON',
    type: 'Transfer',
    note: 'Payment to John',
    status: 'Completed',
    isImportant: false,
    createdAt: '2025-05-13T11:15:00',
    fromAccountId: 1,
    toAccountId: 3,
    fromAccountNumber: 'RO49BANK1000200030004',
    toAccountNumber: 'RO49BANK1000200030006'
  },
  {
    id: 4,
    amount: 120.50,
    currency: 'RON',
    type: 'BillPayment',
    note: 'Electricity bill - Enel - AB123456',
    status: 'Completed',
    isImportant: false,
    createdAt: '2025-05-14T10:05:00',
    fromAccountId: 1,
    fromAccountNumber: 'RO49BANK1000200030004',
    toAccountNumber: 'BILL-Enel-AB123456'
  }
];

const transactionService = {
  // Get transaction history for a specific account
  getAccountTransactions: async (accountId) => {
    try {
      // For development, return mock data
      return mockTransactions.filter(t => 
        t.fromAccountId === accountId || t.toAccountId === accountId
      );
      
      // When ready to connect to backend, use:
      // const response = await api.get(`/transaction/account/${accountId}`);
      // return response.data;
    } catch (error) {
      throw new Error('Failed to fetch transactions');
    }
  },

  // Create a new transaction (transfer money)
  createTransaction: async (transactionData) => {
    try {
      // For development, create a mock transaction
      const newTransaction = {
        id: Date.now(),
        amount: transactionData.amount,
        currency: transactionData.currency,
        type: transactionData.type,
        note: transactionData.note || '',
        status: 'Completed',
        isImportant: false,
        createdAt: new Date().toISOString(),
        fromAccountId: transactionData.fromAccountId,
        toAccountNumber: transactionData.toAccountNumber || '',
        fromAccountNumber: mockTransactions[0].fromAccountNumber // Placeholder
      };
      
      if (transactionData.type === 'Transfer') {
        newTransaction.toAccountId = 999; // Placeholder ID
      }
      
      mockTransactions.push(newTransaction);
      return { transaction: newTransaction, message: 'Transaction completed successfully' };
      
      // When ready to connect to backend, use:
      // const response = await api.post('/transaction', transactionData);
      // return response.data;
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Transaction failed');
    }
  },

  // Mark a transaction as important
  markTransactionAsImportant: async (transactionId) => {
    try {
      // For development, toggle importance in mock data
      const transaction = mockTransactions.find(t => t.id === transactionId);
      if (transaction) {
        transaction.isImportant = !transaction.isImportant;
      }
      return { message: 'Transaction marked as important' };
      
      // When ready to connect to backend, use:
      // const response = await api.put(`/transaction/${transactionId}/important`);
      // return response.data;
    } catch (error) {
      throw new Error('Failed to mark transaction as important');
    }
  }
};

export default transactionService;