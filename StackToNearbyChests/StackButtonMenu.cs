using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using StardewValley;
using StardewModdingAPI;


namespace StackToNearbyChests
{
    class StackButtonMenu : IClickableMenu
    {
        const int _width = 16;
        const int _height = 16;
        

        const int yShift = 70;

        int pageX, pageY, pageWidth, pageHeight;

        ClickableTextureComponent button;

        internal Rectangle buttonBounds { get => button.bounds; }

        public StackButtonMenu(int pageX, int pageY, int pageWidth, int pageHeight) : base(pageX + pageWidth, yShift + pageY + pageHeight / 3 - Game1.tileSize + Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize)
     
        {
            this.pageX = pageX;
            this.pageY = pageY;
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;

            CreateButton();

            Game1.fadeToBlackRect = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);

        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (button.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()))
            {
                StackLogic.StackToNearbyChests(ModEntry.config.Radius);
            }
        }

        private void CreateButton()
        {
            base.allClickableComponents?.Clear();
            button = null;
            
            button = new ClickableTextureComponent("", new Rectangle(pageX + pageWidth, yShift + pageY + pageHeight / 3 - Game1.tileSize + Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), "", "Stack to nearby chests", ModEntry.buttonIcon, Rectangle.Empty, (float)Game1.pixelZoom, false)
            {
                drawShadow = false
            };
            

        }

        private void UpdatePosition()
        {
            CreateButton();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            
            button.draw(spriteBatch);

            if (button.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()))
                IClickableMenu.drawHoverText(spriteBatch, button.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);

        }

        public override void update(GameTime time)
        {
            base.update(time);
        }
        
    }
}