using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsometricMap.Map
{
    public class MapHandler
    {
        //int m_tileHeight, m_tileWidth;
        //Texture2D m_mapTexture;

        private static MapHandler m_Instance = null;

        public static MapHandler GetInstance()
        {
            if(m_Instance == null)
                m_Instance = new MapHandler();

            return m_Instance;
        }

        private MapHandler()
        {

        }

        public Rectangle GetSourceRectByID(int tileID, int imageWidth, int imageHeight, int tileWidth, int tileHeight)
        {
            Rectangle sourceRect = new Rectangle();

            int tilesAccross = imageWidth / tileWidth;
            int tilesTopDown = imageHeight / tileHeight;

            int yPos = tileID / tilesAccross;
            int xPos = tileID % tilesAccross;

            sourceRect.X = xPos * tileWidth;
            sourceRect.Y = yPos * tileHeight;
            sourceRect.Width = tileWidth;
            sourceRect.Height = tileHeight;

            return sourceRect;
        }

        public Vector2 GetTileSetSizeByTiles(Texture2D tileSet, Vector2 tileSize)
        {
            if (tileSet == null || tileSize == null)
                return Vector2.Zero;

            return new Vector2(tileSet.Width / tileSize.X, tileSet.Height / tileSize.Y);

        }
    }
}
