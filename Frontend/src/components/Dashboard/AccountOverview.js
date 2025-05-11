// Detailed account information and management component
import React, { useState } from 'react';
import accountService from '../../services/accountService';

const AccountOverview = ({ account, onUpdate }) => {
  const [showSpendingLimitEdit, setShowSpendingLimitEdit] = useState(false);
  const [newSpendingLimit, setNewSpendingLimit] = useState(account.spendingLimit);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Format date for better readability
  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  // Handle spending limit update
  const handleUpdateSpendingLimit = async () => {
    setError('');
    setLoading(true);

    try {
      await accountService.updateSpendingLimit(account.id, parseFloat(newSpendingLimit));
      setShowSpendingLimitEdit(false);
      onUpdate(); // Refresh parent component
    } catch (err) {
      setError('Failed to update spending limit');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white overflow-hidden shadow-lg rounded-lg">
      <div className="px-4 py-5 sm:p-6">
        <h3 className="text-lg font-medium text-gray-900 mb-4">
          Account Details
        </h3>
        
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        <div className="space-y-4">
          {/* Account number and status */}
          <div>
            <p className="text-sm font-medium text-gray-500">Account Number</p>
            <p className="mt-1 text-lg text-gray-900">{account.accountNumber}</p>
          </div>

          {/* Current balance */}
          <div>
            <p className="text-sm font-medium text-gray-500">Current Balance</p>
            <p className="mt-1 text-2xl font-bold text-gray-900">
              {account.currency} {account.balance.toFixed(2)}
            </p>
          </div>

          {/* Spending limit with edit capability */}
          <div>
            <p className="text-sm font-medium text-gray-500">Daily Spending Limit</p>
            {showSpendingLimitEdit ? (
              <div className="mt-1 flex items-center space-x-2">
                <input
                  type="number"
                  value={newSpendingLimit}
                  onChange={(e) => setNewSpendingLimit(e.target.value)}
                  className="input-field w-32"
                  min="0"
                />
                <button
                  onClick={handleUpdateSpendingLimit}
                  disabled={loading}
                  className="btn-primary"
                >
                  Save
                </button>
                <button
                  onClick={() => setShowSpendingLimitEdit(false)}
                  className="btn-secondary"
                >
                  Cancel
                </button>
              </div>
            ) : (
              <div className="mt-1 flex items-center">
                <p className="text-lg text-gray-900 mr-4">
                  {account.currency} {account.spendingLimit.toFixed(2)}
                </p>
                <button
                  onClick={() => setShowSpendingLimitEdit(true)}
                  className="text-blue-600 hover:text-blue-500"
                >
                  Edit
                </button>
              </div>
            )}
          </div>

          {/* Daily withdrawal limit */}
          <div>
            <p className="text-sm font-medium text-gray-500">Daily Withdrawal Limit</p>
            <p className="mt-1 text-lg text-gray-900">
              {account.currency} {account.dailyWithdrawalLimit.toFixed(2)}
            </p>
          </div>

          {/* Account status and creation date */}
          <div>
            <p className="text-sm font-medium text-gray-500">Status</p>
            <p className="mt-1 text-lg text-gray-900">{account.status}</p>
          </div>

          <div>
            <p className="text-sm font-medium text-gray-500">Created</p>
            <p className="mt-1 text-lg text-gray-900">{formatDate(account.createdAt)}</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccountOverview;