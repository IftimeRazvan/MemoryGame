using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Model;

namespace MemoryGame.ViewModel.Commands
{
    public class GameCommands : BaseVM
    {
        private readonly GameVM _gameVM;
        private Card? _firstFlippedCard;
        private Card? _secondFlippedCard;
        private bool first = true;

        public GameCommands(GameVM gameVM)
        {
            _gameVM = gameVM;
        }

        private ICommand? _flipCardCommand;
        public ICommand? FlipCardCommand
        {
            get
            {
                if (_flipCardCommand == null)
                    _flipCardCommand = new RelayCommand(FlipCard,CanFlipCard);
                return _flipCardCommand;
            }
        }

        private async void FlipCard(object? parameter)
        {
            if(first)
            {
                first = false;
                foreach (var currentCard in _gameVM.Cards)
                {
                    if (currentCard.IsFlipped && !currentCard.IsMatched && _firstFlippedCard == null)
                    {
                        _firstFlippedCard = currentCard;
                    }
                    else if (currentCard.IsFlipped && !currentCard.IsMatched && _secondFlippedCard == null)
                    {
                        _secondFlippedCard = currentCard;
                    }

                }
            }

            if (parameter is Card card)
            {
                if (!card.IsFlipped && !card.IsMatched)
                {
                    card.IsFlipped = true;

                    if (_firstFlippedCard == null)
                    {
                        _firstFlippedCard = card;
                    }
                    else if (_secondFlippedCard == null)
                    {
                        _secondFlippedCard = card;
                    }
                    else
                    {
                        await Task.Delay(500); 

                        if (_firstFlippedCard.ImagePath == _secondFlippedCard.ImagePath)
                        {
                            _firstFlippedCard.IsMatched = _secondFlippedCard.IsMatched = true;
                        }
                        else 
                        {
                            _firstFlippedCard.IsFlipped = _secondFlippedCard.IsFlipped = false;
                        }
                            
                        _firstFlippedCard = card;
                        _secondFlippedCard = null;
                    }
                }
            }
        }

        private bool CanFlipCard(object? parameter)
        {
            if (parameter is Card card)
            {
                return !card.IsFlipped && !card.IsMatched;
            }
            return false;
        }
    }
}
