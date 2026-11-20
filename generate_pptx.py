import os
from pptx import Presentation
from pptx.util import Inches, Pt
from pptx.dml.color import RGBColor

# Paths
logo_path = r"C:/Users/HP/.gemini/antigravity/brain/c3bbca32-cf81-4e24-bce3-a67fd68a5b7f/.user_uploaded/media_1795168130915_7b097cea.jpg"
output_path = r"C:/Users/HP/source/repos/Gestion_Universite/Gestion_Universite.pptx"

# Helper to add a slide with title layout
def add_title_slide(prs, title, subtitle=None):
    slide_layout = prs.slide_layouts[0]  # Title Slide
    slide = prs.slides.add_slide(slide_layout)
    slide.shapes.title.text = title
    if subtitle:
        slide.placeholders[1].text = subtitle
    return slide

# Helper to add a bullet slide
def add_bullet_slide(prs, title, bullet_points):
    slide_layout = prs.slide_layouts[1]  # Title and Content
    slide = prs.slides.add_slide(slide_layout)
    slide.shapes.title.text = title
    tf = slide.placeholders[1].text_frame
    for i, pt in enumerate(bullet_points):
        p = tf.add_paragraph() if i else tf.paragraphs[0]
        p.text = pt
        p.level = 0
    return slide

# Create presentation
prs = Presentation()

# Define theme colors (red, white)
red_rgb = RGBColor(0x8B, 0x00, 0x00)
white_rgb = RGBColor(0xFF, 0xFF, 0xFF)

# Title slide with logo
slide = add_title_slide(prs, "Gestion Universitaire – Présentation", "IIT – Système de Gestion Universitaire")
if os.path.exists(logo_path):
    left = Inches(7)
    top = Inches(0.5)
    height = Inches(1.5)
    slide.shapes.add_picture(logo_path, left, top, height=height)

# Team slide
team_names = ["EHOUSSOU PHANUEL", "SIHOULI HOREB", "SAKI DJOROU", "OUATTARA NOURDINE"]
slide = add_bullet_slide(prs, "Équipe du projet", [f"{name}" for name in team_names])

# Sections
sections = [
    ("Introduction", [
        "Contexte du système de gestion universitaire",
        "Objectifs principaux : suivi des étudiants, gestion des cours, emplois du temps",
        "Palette de couleur : rouge #8B0000 et blanc"
    ]),
    ("À propos du projet", [
        "Architecture N‑tiers (Présentation, DAO, DataAccess)",
        "Technologies : WinForms .NET 10, SQL Server",
        "Modules clés : Années académiques, Filières, Niveaux, Classes, Étudiants, Inscriptions, Enseignants, Cours, Salles, Emplois du temps, Absences"
    ]),
    ("Présentation du projet", [
        "Point d’entrée : Menu principal affichant le tableau de bord (statistiques)",
        "Flux principal : voir les listes → détailler → ajouter/modifier → valider les règles métier (double inscription, conflits d’emploi du temps)",
        "Point de sortie : fermeture de l’application après sauvegarde automatique dans la base SQL Server",
        "Approche : séparation Liste / Détail, navigation latérale, validation côté DAO"
    ]),
    ("Conclusion", [
        "Solution robuste pour la gestion quotidienne d’une université",
        "Facilité d’extension grâce à la couche DAO",
        "Respect de la charte graphique IIT (rouge/crimson et blanc)"
    ])
]

for title, bullets in sections:
    add_bullet_slide(prs, title, bullets)

# Save presentation
prs.save(output_path)
print(f"Presentation saved to {output_path}")
