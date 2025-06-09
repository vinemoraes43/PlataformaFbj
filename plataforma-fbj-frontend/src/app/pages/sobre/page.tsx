import { Header } from '@/components/Header'

export default function SobrePage() {
    return (
        <div>
            <Header />
            
            <div className="container mx-auto px-4 py-12">
                <div className="max-w-3xl mx-auto text-center">
                    <h1 className="text-4xl font-bold mb-6">Sobre o GameReviews</h1>
                    
                    <div className="prose prose-invert max-w-none">
                        <p className="text-xl mb-6">
                            O GameReviews nasceu da paixão por jogos e da necessidade de criar um espaço onde jogadores 
                            podem compartilhar suas experiências genuínas sobre os jogos que amam (ou não amam).
                        </p>
                        
                        <p className="mb-6">
                            Em um mundo cheio de análises profissionais, acreditamos que as opiniões de jogadores reais 
                            são igualmente valiosas. Aqui, cada jogador tem voz e cada avaliação importa.
                        </p>
                        
                        <h2 className="text-2xl font-bold mt-10 mb-4">Nossa Missão</h2>
                        <p className="mb-6">
                            Criar a maior comunidade de avaliações de jogos feitas por jogadores, para jogadores. 
                            Queremos ajudar você a encontrar seu próximo jogo favorito baseado nas experiências 
                            de pessoas como você.
                        </p>
                        
                        <h2 className="text-2xl font-bold mt-10 mb-4">Como Funciona</h2>
                        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
                            <div className="bg-gray-800 p-6 rounded-xl">
                                <div className="text-4xl mb-4">1</div>
                                <h3 className="text-xl font-bold mb-2">Cadastre-se</h3>
                                <p>Crie sua conta gratuitamente em segundos</p>
                            </div>
                            <div className="bg-gray-800 p-6 rounded-xl">
                                <div className="text-4xl mb-4">2</div>
                                <h3 className="text-xl font-bold mb-2">Avalie</h3>
                                <p>Dê notas e escreva análises para os jogos que jogou</p>
                            </div>
                            <div className="bg-gray-800 p-6 rounded-xl">
                                <div className="text-4xl mb-4">3</div>
                                <h3 className="text-xl font-bold mb-2">Compartilhe</h3>
                                <p>Conecte-se com outros jogadores e descubra novos jogos</p>
                            </div>
                        </div>
                        
                        <div className="mt-12">
                            <h2 className="text-2xl font-bold mb-4">Junte-se a Nós</h2>
                            <p className="mb-6">
                                Faça parte da comunidade que está transformando a maneira como descobrimos jogos.
                            </p>
                            <button className="bg-yellow-500 hover:bg-yellow-600 text-gray-900 font-bold py-3 px-8 rounded-full text-lg">
                                Criar Minha Conta
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}