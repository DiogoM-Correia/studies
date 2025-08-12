import React, { useState } from 'react';
import Navigation from './components/Navigation';
import MainPage from './pages/MainPage';
import AboutPage from './pages/AboutPage';
import TeamPage from './pages/TeamPage';
import ServicesPage from './pages/ServicesPage';
import './App.css';

type PageType = 'main' | 'about' | 'team' | 'services';

const App: React.FC = () => {
  const [currentPage, setCurrentPage] = useState<PageType>('main');

  const renderPage = () => {
    switch (currentPage) {
      case 'main':
        return <MainPage />;
      case 'about':
        return <AboutPage />;
      case 'team':
        return <TeamPage />;
      case 'services':
        return <ServicesPage />;
      default:
        return <MainPage />;
    }
  };

  return (
    <div className="app">
      <Navigation currentPage={currentPage} onPageChange={setCurrentPage} />
      <main className="main-content">
        {renderPage()}
      </main>
    </div>
  );
};

export default App;
