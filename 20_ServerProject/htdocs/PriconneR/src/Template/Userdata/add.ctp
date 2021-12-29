<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Userdata $userdata
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('List Userdata'), ['action' => 'index']) ?></li>
    </ul>
</nav>
<div class="userdata form large-9 medium-8 columns content">
    <?= $this->Form->create($userdata) ?>
    <fieldset>
        <legend><?= __('Add Userdata') ?></legend>
        <?php
            echo $this->Form->control('name');
            echo $this->Form->control('rank');
            echo $this->Form->control('exp');
            echo $this->Form->control('lastlogin');
        ?>
    </fieldset>
    <?= $this->Form->button(__('Submit')) ?>
    <?= $this->Form->end() ?>
</div>
