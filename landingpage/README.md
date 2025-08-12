# Landing Page Website

A modern, responsive landing page built with React, TypeScript, and Vite. Features smooth navigation between 4 pages with beautiful animations and responsive design.

## Features

- **4 Pages**: Main, About Us, Team, and Services
- **Top-right Navigation**: Fixed navigation menu for easy page switching
- **Responsive Design**: Works perfectly on all devices
- **Modern UI**: Beautiful gradients, animations, and hover effects
- **TypeScript**: Full type safety and modern development experience
- **Fast Development**: Built with Vite for instant hot reload

## Pages

### Main Page
- Hero section with animated image (GIF)
- Company title and subtitle
- Beautiful gradient background

### About Us
- Same animated image
- Company description
- Contact information (phone, email, Instagram, LinkedIn)

### Team
- Team member cards with photos
- Names, LinkedIn profiles, and descriptions
- Hover effects and animations

### Services
- Service cards with photos
- Titles and descriptions
- Responsive grid layout

## Tech Stack

- **React 18** - Modern React with hooks
- **TypeScript** - Type-safe development
- **Vite** - Fast build tool and dev server
- **CSS3** - Modern styling with gradients and animations
- **Responsive Design** - Mobile-first approach

## Getting Started

### Prerequisites
- Node.js (version 16 or higher)
- npm or yarn

### Installation

1. **Clone or download** the project files
2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Start development server**:
   ```bash
   npm run dev
   ```

4. **Open your browser** and navigate to `http://localhost:3000`

### Build for Production

```bash
npm run build
```

The built files will be in the `dist` folder, ready for deployment.

### Preview Production Build

```bash
npm run preview
```

## Project Structure

```
src/
├── components/          # Reusable components
│   ├── Navigation.tsx  # Top navigation menu
│   └── Navigation.css
├── pages/              # Page components
│   ├── MainPage.tsx    # Main landing page
│   ├── AboutPage.tsx   # About us page
│   ├── TeamPage.tsx    # Team members page
│   ├── ServicesPage.tsx # Services page
│   └── *.css          # Page-specific styles
├── App.tsx             # Main app component
├── App.css             # App-level styles
├── main.tsx            # Entry point
└── index.css           # Global styles
```

## Customization

### Changing Content
- **Text**: Edit the content in each `.tsx` file
- **Images**: Replace the image URLs with your own
- **Colors**: Modify the CSS variables and gradients
- **Layout**: Adjust the CSS grid and flexbox properties

### Adding New Pages
1. Create a new page component in `src/pages/`
2. Add the page type to the `PageType` union in `App.tsx`
3. Add the page to the navigation and routing logic
4. Create corresponding CSS file

## Deployment

The built project can be deployed to any static hosting service:

- **Netlify** - Drag and drop the `dist` folder
- **Vercel** - Connect your GitHub repository
- **GitHub Pages** - Upload the `dist` folder
- **AWS S3** - Upload files to an S3 bucket
- **Any web server** - Upload files to your server

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
- Mobile browsers (iOS Safari, Chrome Mobile)

## License

This project is open source and available under the MIT License.

## Support

For questions or issues, please check the code comments or create an issue in the repository.
