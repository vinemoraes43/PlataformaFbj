"use client"

import { usePathname, useRouter } from "next/navigation"
import { useEffect, useState } from "react"
import Link from 'next/link'

type NavOption = 'home' | 'jogos' | 'avaliacoes' | 'ranking' | 'sobre'

export const Header = () => {
    const router = useRouter()
    const pathname = usePathname()
    const [activeOption, setActiveOption] = useState<NavOption>('home')

    useEffect(() => {
        const currentOption = pathname.split('/')[1] || 'home'
        setActiveOption(currentOption as NavOption)
    }, [pathname])

    const handleNavigation = (option: NavOption) => {
        router.push(`/${option}`)
        setActiveOption(option)
    }

    const navOptions = [
        { id: 'home', label: 'Início' },
        { id: 'jogos', label: 'Jogos' },
        { id: 'avaliacoes', label: 'Avaliações' },
        { id: 'ranking', label: 'Ranking' },
        { id: 'sobre', label: 'Sobre' }
    ]
    
    return (
        <header className="w-full bg-gray-900 text-white p-4 shadow-xl sticky top-0 z-50">
            <div className="container mx-auto flex flex-col md:flex-row justify-between items-center">
                <div className="flex items-center mb-4 md:mb-0">
                    <Link href="/" className="flex items-center">
                        <div className="bg-yellow-500 text-gray-900 font-bold p-2 rounded mr-3">
                            <span className="text-xl">🎮</span>
                        </div>
                        <h1 className="text-2xl font-bold">Game<span className="text-yellow-400">Reviews</span></h1>
                    </Link>
                </div>

                <nav className="flex flex-wrap justify-center gap-2">
                    {navOptions.map((option) => (
                        <button
                            key={option.id}
                            className={`px-4 py-2 rounded-lg transition-all ${
                                activeOption === option.id
                                    ? 'bg-yellow-500 text-gray-900 font-bold'
                                    : 'bg-gray-700 hover:bg-gray-600'
                            }`}
                            onClick={() => handleNavigation(option.id as NavOption)}
                        >
                            {option.label}
                        </button>
                    ))}
                </nav>

                <div className="mt-4 md:mt-0 flex items-center">
                    <button 
                        className="bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg flex items-center"
                        onClick={() => router.push('/login')}
                    >
                        <span className="mr-2">👤</span> Entrar
                    </button>
                </div>
            </div>
        </header>
    )
}