import React from 'react';
import './Navigation.css';

interface NavigationProps {
  currentPage: string;
  onPageChange: (page: 'main' | 'about' | 'team' | 'services') => void;
}

const Navigation: React.FC<NavigationProps> = ({ currentPage, onPageChange }) => {
  const pages = [
    { id: 'main', label: 'Main' },
    { id: 'team', label: 'Team' },
    { id: 'services', label: 'Services' },
    { id: 'about', label: 'About Us' }
  ] as const;

  return (
    <nav className="navigation">
      <div className="nav-container">
        <div className="logo-section">
          <div className="logo">
            <span className="logo-text">LOGO</span>
          </div>
        </div>
        <ul className="nav-list">
          {pages.map((page) => (
            <li key={page.id} className="nav-item">
              <button
                className={`nav-button ${currentPage === page.id ? 'active' : ''}`}
                onClick={() => onPageChange(page.id)}
              >
                {page.label}
              </button>
            </li>
          ))}
        </ul>
      </div>
    </nav>
  );
};

export default Navigation;
