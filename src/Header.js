// Header.js - The navigation guide for your application
import React from 'react';

const Header = () => {
    return (
        <header className="bg-white shadow">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex justify-between h-16">
                    <div className="flex">
                        <div className="flex-shrink-0 flex items-center">
                            <a href="/public" className="text-2xl font-bold text-blue-600">
                                BankApp
                            </a>
                        </div>
                    </div>

                    <div className="flex items-center">
                        <button className="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded">
                            Logout
                        </button>
                    </div>
                </div>
            </div>
        </header>
    );
};

export default Header;