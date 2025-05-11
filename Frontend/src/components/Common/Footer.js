import React from 'react';

const Footer = () => {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="bg-gray-800 text-white pt-8 pb-4 mt-auto">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          {/* Company Info */}
          <div>
            <h3 className="text-xl font-bold text-white mb-4">BankApp</h3>
            <p className="text-gray-300">Your secure banking solution for everyday financial needs</p>
            <p className="text-gray-300 mt-2">Licensed and regulated by the National Bank of Romania</p>
          </div>
          
          {/* Quick Links */}
          <div>
            <h3 className="text-xl font-bold text-white mb-4">Quick Links</h3>
            <ul className="space-y-2">
              <li><a href="/dashboard" className="text-gray-300 hover:text-white hover:underline">Dashboard</a></li>
              <li><a href="/help" className="text-gray-300 hover:text-white hover:underline">Help Center</a></li>
              <li><a href="/security" className="text-gray-300 hover:text-white hover:underline">Security</a></li>
              <li><a href="/terms" className="text-gray-300 hover:text-white hover:underline">Terms & Conditions</a></li>
              <li><a href="/privacy" className="text-gray-300 hover:text-white hover:underline">Privacy Policy</a></li>
            </ul>
          </div>
          
          {/* Contact Info */}
          <div>
            <h3 className="text-xl font-bold text-white mb-4">Contact Us</h3>
            <p className="text-gray-300 mb-2">Email: support@bankapp.com</p>
            <p className="text-gray-300 mb-2">Phone: +40 123 456 789</p>
            <p className="text-gray-300">Address: Str. Victoriei 12, Craiova, Romania</p>
            
            <div className="mt-4 flex space-x-4">
              <a href="#facebook" className="text-gray-300 hover:text-white">
                <span className="sr-only">Facebook</span>
                {/* You can add icons here if needed */}
                <span>FB</span>
              </a>
              <a href="#twitter" className="text-gray-300 hover:text-white">
                <span className="sr-only">Twitter</span>
                <span>TW</span>
              </a>
              <a href="#linkedin" className="text-gray-300 hover:text-white">
                <span className="sr-only">LinkedIn</span>
                <span>LI</span>
              </a>
            </div>
          </div>
        </div>
        
        {/* Copyright */}
        <div className="border-t border-gray-700 mt-8 pt-6 text-center">
          <p className="text-sm text-gray-400">&copy; {currentYear} BankApp. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
};

export default Footer;