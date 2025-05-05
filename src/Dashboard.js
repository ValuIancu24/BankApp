import React, { useState } from 'react';
import './styles.css';

const Dashboard = () => {
    const [accounts] = useState([
        { id: 1, accountNumber: 'RO87654321', currency: 'RON', balance: 58560, status: 'Inactive' },
        { id: 2, accountNumber: 'RO12345678', currency: 'USD', balance: 10756750, status: 'Active' },
        { id: 3, accountNumber: 'RO98765432', currency: 'EUR', balance: 8756756750, status: 'Active' },
        {id : 4 , accountNumber: 'RO12345678', currency: 'USD', balance: 10756750, status: 'Active' }
    ]);



    const handleTransfer = () => alert('Transfer Money clicked!');
    const handleCreateAccount = () => alert('Create Account clicked!');
    const handlePayBills = () => alert('Pay Bills clicked!');
    const handleViewTransactions = (accountId) => alert(`View transactions for account ${accountId}`);
    const handleManageAccount = (accountId) => alert(`Manage account ${accountId}`);

    return (
        <div className="app">
            <header className="header">
                <h1>BankApp</h1>


                <button className="logout-btn">Logout  </button>
            </header>

            <main className="main-content">
                <h2>Welcome to Your BankApp Dashboard</h2>



                <div className="action-buttons">
                    <button className="btn btn-blue" onClick={handleTransfer}>Transfer Money</button>
                    <button className="btn btn-green" onClick={handleCreateAccount}>Create Account</button>



                    <button className="btn btn-purple" onClick={handlePayBills}>Pay Bills</button>
                </div>

                <div className="accounts-grid">


                    {accounts.map(account => (
                        <div key={account.id} className="account-card">
                            <h3>{account.currency} Account</h3>
                            <p className="account-number">{account.accountNumber}</p>
                            <p className="balance">{account.currency} {account.balance.toFixed(2)}</p>

                            <div className="account-actions">
                                <button
                                    className="btn btn-blue"
                                    onClick={() => handleViewTransactions(account.id)}
                                >
                                    View Transactions
                                </button>
                                <button



                                    className="btn btn-gray"
                                    onClick={() => handleManageAccount(account.id)}
                                >
                                    Manage Account
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </main>
        </div>



    );
};

export default Dashboard;